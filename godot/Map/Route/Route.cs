namespace ForForm.Map.Route
{
    using Godot;

    [Tool,Icon("res://Script icons/conversion_path.png")]
    public partial class Route : Path3D {
        /// WARN: Remember: when changing or adding any variable add that to RouteExport.cs
        [ExportGroup("Input by hand")]
        [Export]
        public string name;
        [Export(PropertyHint.MultilineText)]
        public string description;


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
