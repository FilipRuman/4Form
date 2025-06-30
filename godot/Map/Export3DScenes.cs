namespace ForForm.Map
{
    using Godot;

    [Icon("res://Script icons/publish.png")]
    [Tool]
    public partial class Export3DScenes : Node3D {
        Map map;

        public void ImportScene(Map _map) {
            map = _map;
            var gltfDocumentLoad = new GltfDocument();
            var gltfStateLoad = new GltfState();
            var error = gltfDocumentLoad.AppendFromFile(SceneBasePath + "Scene.glb", gltfStateLoad);

            if (error == Error.Ok) {
                var gltfSceneRootNode = gltfDocumentLoad.GenerateScene(gltfStateLoad);
                var d = gltfSceneRootNode as Node3D;
                AddChild(d);
                d.Owner = Owner;

                d.Visible = true;
            } else {
                GD.PrintErr($"Couldn't load glTF scene (error code: {error}).");
            }
        }

        string SceneBasePath => $"user://Maps/{map.name}/Scene/";

        public void ExportScene(Map _map) {
            map = _map;
            DirAccess.Open(SceneBasePath).Remove("textures");
            var gltfDocumentSave = new GltfDocument();
            var gltfStateSave = new GltfState();
            gltfDocumentSave.AppendFromScene(this, gltfStateSave);

            gltfDocumentSave.WriteToFilesystem(gltfStateSave, SceneBasePath + "Scene.glb");
        }
    }
}
