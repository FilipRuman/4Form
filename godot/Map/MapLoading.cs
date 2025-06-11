namespace ForForm.Map
{
    using Godot;

    public partial class MapLoading : Node {
        [Export]
        Node mapParent;

        public Route.Route[] Load(PackedScene mapPrefab) {
            Menu.UIMiscs.ClearChildren(mapParent);
            var map = mapPrefab.Instantiate() as Map;
            mapParent.AddChild(map);
            GameConfig.GameSettings.currentMap = map;
            return map.routes;
        }
    }
}
