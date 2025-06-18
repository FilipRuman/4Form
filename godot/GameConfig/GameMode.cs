namespace ForForm.GameConfig
{
    using Godot;

    [GlobalClass,Tool]
    public partial class GameMode : Resource {
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

    }
}
