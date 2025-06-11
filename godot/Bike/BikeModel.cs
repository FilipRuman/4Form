namespace ForForm.Bike
{
    using Godot;

    [GlobalClass]
    public partial class BikeModel : Resource {
        [Export]
        public string name;

        [Export]
        public float mass, //kg
            wheelFrictionCoefficient,
            wheelRadius, //m
            frontalArea;

        [Export]
        public Texture2D icon;
    }
}
