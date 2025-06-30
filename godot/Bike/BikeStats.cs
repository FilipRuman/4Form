namespace ForForm.Bike
{
    public static class BikeStats {
        public static BikeModel bikeModel;

        public static float drag => dragCoefficient * (frontalArea_m + bikeModel.frontalArea_m);
        public static float dragCoefficient;
        public static float frontalArea_m;
        public static float totalMass_kg => userConfig.mass_kg + bikeModel.mass_kg;
    }
}
