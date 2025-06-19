namespace ForForm.Map
{
    using Godot;

    [Icon("res://Script icons/publish.png")]
    [Tool]
    public partial class WholeMapExport : Node {
        [Export]
        Route.RouteExport routeExport;

        [Export]
        Node terrain3DTrueExport;

        [Export]
        Map map;

        [Export]
        bool export,
            clearChildren,
            manualMapLoad;

        [Export]
        string manualMapLoadName;

        public override void _Process(double delta) {
            SetMeta("_edit_lock_", true);
            if (export) {
                export = false;
                Export();
            }
            if (manualMapLoad) {
                manualMapLoad = false;
                Import(manualMapLoadName);
            }
            if (clearChildren) {
                Menu.UIMiscs.ClearChildren(this);
                clearChildren = false;
            }

            base._Process(delta);
        }

        /// Call this to import ALL components of map like:
        /// map, game mode, 3D scene, terrain 3D, routes
        public void Import(string mapName) {
            map.name = mapName;

            terrain3DTrueExport.Call("run_import");
            GameConfig.GameSettings.currentMap = Map.Load(mapName);

            GameConfig.GameSettings.currentMap.gameMode = GameConfig.GameMode.Load(mapName);
            GameConfig.GameSettings.SetCurrentGameMode(GameConfig.GameSettings.currentMap.gameMode);
            Menu.UIMiscs.ClearChildren(this);
            ImportScene();
            routeExport.ImportRoutes();
        }

        public void ImportScene() {
            var gltfDocumentLoad = new GltfDocument();
            var gltfStateLoad = new GltfState();
            var error = gltfDocumentLoad.AppendFromFile(SceneBasePath + "Scene.glb", gltfStateLoad);

            if (error == Error.Ok) {
                var gltfSceneRootNode = gltfDocumentLoad.GenerateScene(gltfStateLoad);
                AddChild(gltfSceneRootNode);
                if (Engine.IsEditorHint())
                    gltfSceneRootNode.Owner = map;
                var d = gltfSceneRootNode as Node3D;
                HandleImportedNode(this, d);

                d.Visible = true;
            } else {
                GD.PrintErr($"Couldn't load glTF scene (error code: {error}).");
            }
        }

        void HandleImportedNode(Node parent, Node importNode) {
            if (importNode is ImporterMeshInstance3D importerMesh) {
                var realMesh = new MeshInstance3D();
                parent.AddChild(realMesh);
                realMesh.MaterialOverlay = importerMesh.Mesh.GetSurfaceMaterial(0);
                realMesh.Transform = importerMesh.Transform;
                realMesh.Mesh = importerMesh.Mesh.GetMesh();
                foreach (var child in importNode.GetChildren()) {
                    HandleImportedNode(realMesh, child);
                }
            } else {
                var realNode = new Node3D();
                realNode.Transform = (importNode as Node3D).Transform;
                parent.AddChild(realNode);
                foreach (var child in importNode.GetChildren()) {
                    HandleImportedNode(realNode, child);
                }
            }
        }

        /// Call this to export ALL components of map like:
        /// map, game mode, 3D scene, terrain 3D, routes

        public void Export() {
            DirAccess.Open($"user://Maps/{map.name}/").MakeDir("Scene");

            map.Save();
            map.gameMode.Save(map.name);

            terrain3DTrueExport.Call("run_export");
            ExportScene();
            routeExport.ExportRoutes();
        }

        string SceneBasePath => $"user://Maps/{map.name}/Scene/";

        public void ExportScene() {
            DirAccess.Open(SceneBasePath).Remove("textures");
            var gltfDocumentSave = new GltfDocument();
            var gltfStateSave = new GltfState();
            gltfDocumentSave.AppendFromScene(this, gltfStateSave);

            gltfDocumentSave.WriteToFilesystem(gltfStateSave, SceneBasePath + "Scene.glb");
        }
    }
}
