namespace ForForm.Bike
{
    using Godot;

    public partial class BikePath : Node {
        [Export]
        BikePhysics bikePhysics;

        [Export]
        PathFollow3D pathFollow;
        float distanceFromStart;

        public float slope;
        public float slopeAngleRad;

        public override void _Ready() {
            bikePhysics.Progress = GameConfig.GameSettings.currentRoute.startingPoint;

            base._Ready();
        }

        public override void _Process(double delta) {
            distanceFromStart +=
                bikePhysics.speed * (float)delta * GameConfig.GameSettings.currentMap.speedScale;
            // this shouldn't happen but just to make sure...

            float whealBase = BikeStats.bikeModel.modelsWheelBase;

            pathFollow.Progress = distanceFromStart;
            var backWheal = bikePhysics.Position.Y;
            pathFollow.Progress = distanceFromStart + whealBase;
            var frontWheal = bikePhysics.Position.Y;

            var heightDelta = frontWheal - backWheal;
            slopeAngleRad = Mathf.Atan(heightDelta / whealBase); // rad
            slope = Mathf.Tan(slopeAngleRad) * 100; // %

            bikePhysics.Progress = distanceFromStart;

            var rotation = bikePhysics.GlobalRotationDegrees;
            // Z = 0 So the bike doesn't drive on it's side because of path weirdness
            rotation = new(Mathf.RadToDeg(slopeAngleRad), rotation.Y, 0);
            bikePhysics.GlobalRotationDegrees = rotation;
            base._Process(delta);
        }
    }
}
