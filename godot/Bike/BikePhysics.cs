namespace ForForm.Bike
{
    using Godot;

    // I could just use the bike trainer simulation mode but:
    // 1. this allows for SO MUCH more customization
    // 2. allows you to use this app even if you don't have a smart trainer - just 'dumb' one + power meter pedals.
    // 4. if you don't want to change gears you can drive in egr mode.
    // 5. we don't have to rely on the bike trainer simulation. just a good power sensor.
    // 6. it's not a lot of code.

    public partial class BikePhysics : PathFollow3D {
        // I put units after _ in camel case so: speed in km/h will be speed_(first word lowercase)km(second word Capitalize)H
        // so you know the unit at firs look and you don't make mistakes with them!
        [Export]
        public Camera3D camera;

        [Export]
        public BikeInput input;

        [Export]
        public HUD.BikeHudMain hudMain;

        [Export]
        public float speed_mS; //m/s
        public float speed_kmH => speed_mS * 3.6f; // km/h
        public BikeModel bikeModel;
        public float userMass_kg;

        [Export]
        float gravity_mS2; //m/s^2
        public float testingPower = 0;

        // I could use formula from my flight sim XD https://github.com/FilipRuman/Flight-sim
        const float StandardAirDensity_kgM3 = 1.2250f; // kg/m^3

        public Map.Route.Route route;
        public Map.Map map;

        public float Acceleration_mS2() {
            float gravity_N = (userMass_kg + bikeModel.mass_kg) * gravity_mS2;
            float slopeGravityForce_N = gravity_N * Mathf.Sin(slope_rad); //the force that is pushing you forward from hills

            // If you take a corner that looks like nascar track (curved to the inside) the we would need to account other forces but this doesn't matter
            float normalGravityForce_N = gravity_N * Mathf.Cos(slope_rad); //the force that is applied directly to the ground
            float rollingResistance_N = bikeModel.wheelFrictionCoefficient * normalGravityForce_N; //N

            float airDrag_N =
                bikeModel.frontalArea_m
                * map.dragCoefficient
                * StandardAirDensity_kgM3
                * Mathf.Pow(speed_mS, 2)
                / 2f; //N

            //https://en.wikipedia.org/wiki/Torque
            float drivetrainForwardPushing_N = input.currentWatts / Mathf.Max(speed_mS, 1); // N

            float totalForwardForce_N =
                drivetrainForwardPushing_N
                + testingPower / Mathf.Max(speed_mS, 1)
                - slopeGravityForce_N
                - rollingResistance_N
                - airDrag_N; //N

            return totalForwardForce_N / (userMass_kg + bikeModel.mass_kg); // m/s^2 clamped to remove any weirdness
        }

        public override void _PhysicsProcess(double delta) {
            // so you don't roll backwards on hills when stopping pedaling
            speed_mS = Mathf.Max(speed_mS + Acceleration_mS2() * (float)delta, .001f);

            UpdatePath(delta);
            base._PhysicsProcess(delta);
        }

        private void UpdatePath(double delta) {
            float currentProgress_m = Progress + speed_mS * (float)delta * map.speedScale;
            CalculateSlope(currentProgress_m);

            Progress = currentProgress_m;

            //TODO: later add rotation at Z axis to add animation for rotating into turns
            Vector3 rotation_deg = new(Mathf.RadToDeg(slope_rad), GlobalRotationDegrees.Y, 0);
            GlobalRotationDegrees = rotation_deg;
        }

        private void CalculateSlope(float currentProgress_m) {
            Progress = currentProgress_m;
            var backWheal = Position.Y;
            Progress = currentProgress_m + bikeModel.modelsWheelBase_m;
            var frontWheal = Position.Y;

            var heightDelta = frontWheal - backWheal;
            slope_rad = Mathf.Atan(heightDelta / bikeModel.modelsWheelBase_m);
            slope_percent = Mathf.Tan(slope_rad) * 100;
        }

        public float slope_percent; //%
        public float slope_rad;

        public override void _Ready() {
            Progress = route.startingPoint;
            base._Ready();
        }
    }
}
