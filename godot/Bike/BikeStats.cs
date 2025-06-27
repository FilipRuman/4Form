namespace ForForm.Bike
{
    public static class BikeStats {
        public static BikeModel bikeModel;
        public static Player.UserConfig userConfig;

        public static float drag => dragCoefficient * (frontalArea_m + bikeModel.frontalArea_m);
        public static float dragCoefficient;
        public static float frontalArea_m;
        public static float totalMass_Kg => userConfig.mass_kg + bikeModel.mass_kg; //kg
    }
}
