namespace ForForm.Bike
{
    using Godot;

    public partial class BikeStats : Node {
        [Export]
        public float bikeMass; //kg

        [Export]
        public float userMass; //kg
        public float totalMass => bikeMass + userMass; //kg

        [Export]
        public float whealFrictionCoefficient;

        [Export]
        public float whealRadius; //m

        // public float whealCircumference => whealRadius * 2 * Mathf.Pi;

        [Export]
        public float frontalArea;

        [Export]
        public float dragCoefficient;
    }
}
