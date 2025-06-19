namespace ForForm.Bike
{
    public static class BikeStats {
        public static BikeModel bikeModel;
        public static Player.UserConfig userConfig;

        public static float drag => dragCoefficient * (frontalArea + bikeModel.frontalArea);
        public static float dragCoefficient;
        public static float frontalArea;
        public static float totalMass => userConfig.mass + bikeModel.mass; //kg
    }
}
