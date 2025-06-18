namespace ForForm.Bike
{

    public static class BikeStats {
        public static BikeModel bikeModel;
        public static Player.UserConfig userConfig;

        // public float whealCircumference => whealRadius * 2 * Mathf.Pi;
        public static float drag => dragCoefficient * (frontalArea + bikeModel.frontalArea);
        public static float dragCoefficient;
        public static float frontalArea;
    }
}
