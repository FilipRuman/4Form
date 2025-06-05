namespace ForForm.Bike
{
    using Godot;

    public static class BikeStats {
        public static BikeModel bikeModel;
        public static float userMass; //kg
        public static float totalMass => bikeModel.mass + userMass; //kg

        // public float whealCircumference => whealRadius * 2 * Mathf.Pi;
        public static float drag => dragCoefficient * (frontalArea + bikeModel.frontalArea);
        public static float dragCoefficient;
        public static float frontalArea;
    }
}
