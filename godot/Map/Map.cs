namespace ForForm.Map
{
    using Godot;

    public partial class Map : Node3D {
        [Export]
        public Route.Route[] routes;

        [Export]
        public Node3D terrain3D;
    }
}
