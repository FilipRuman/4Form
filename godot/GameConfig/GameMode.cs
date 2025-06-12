namespace ForForm.GameConfig
{
    using Godot;

    [GlobalClass]
    public partial class GameMode : Resource {
        [Export]
        public string name;

        [Export]
        public Texture2D icon;

        [Export(PropertyHint.MultilineText)]
        public string description;

        [ExportGroup("bike models")]
        [Export]
        public bool canEditBikeModels;

        [Export]
        public Bike.BikeModel[] bikeModels;

        [ExportGroup("drag settings")]
        [Export]
        public float dragCoefficient = 0;

        [Export]
        public float userDrag = 0;

        [ExportGroup("map")]
        [Export]
        public PackedScene map;
    }
}
