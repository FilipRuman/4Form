namespace ForForm.Map
{
    using Godot;

    [Icon("res://Script icons/publish.png")]
    [Tool]
    public partial class WholeMapExport : Node {
        [Export]
        Route.RouteExport routeExport;

        [Export]
        Export3DScenes export3DScenes;

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
                Miscs.ClearChildren(this);
                clearChildren = false;
            }

            base._Process(delta);
        }

        /// Call this to import ALL components of map like:
        /// map, game mode, 3D scene, terrain 3D, routes
        public Map Import(string mapName) {
            map.name = mapName;

            map = Map.Load(mapName);

            Miscs.ClearChildren(this);
            export3DScenes.ImportScene(map);
            routeExport.ImportRoutes(map);

            terrain3DTrueExport.Call("run_import");
            return map;
        }

        /// Call this to export ALL components of map like:
        /// map, game mode, 3D scene, terrain 3D, routes
        public void Export() {
            OS.MoveToTrash(ProjectSettings.GlobalizePath($"user://Maps/{map.name}/"));
            DirAccess.Open($"user://Maps/").MakeDir(map.name);
            DirAccess.Open($"user://Maps/{map.name}/").MakeDir("Scene");

            map.Save();

            terrain3DTrueExport.Call("run_export");
            export3DScenes.ExportScene(map);
            routeExport.ExportRoutes(map);
        }
    }
}
