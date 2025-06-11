namespace ForForm.Map.Route
{
    using Godot;

    public partial class Route : Path3D {
        [ExportGroup("Input by hand")]
        [Export]
        public string name;

        [Export]
        public Texture2D icon;

        [Export]
        public float startingPoint;

        [Export]
        public string difficulty;

        [Export]
        public uint estimatedTime; // min

        [ExportGroup("Read only")]
        ///TODO:
        [Export]
        public float totalDistance; // m

        /// m
        [Export]
        public float ascent;

        // Checkpoints
        // Connections to other routes
    }
}
