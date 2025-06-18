namespace ForForm.Map
{
    using Godot;

    [Tool]
    public partial class Map : Node3D {
        [Export]
        public string name;

        [Export(PropertyHint.MultilineText)]
        public string description;

        [Export]
        public Texture2D icon;

        [Export]
        public GameConfig.GameMode gameMode;

        [Export]
        public Route.Route[] routes;

        [Export]
        public Node3D terrain3D;
    }
}
