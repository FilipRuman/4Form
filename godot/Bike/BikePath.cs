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
        public float slopeAngleRadians;

        [Export]
        float speedScale;

        public override void _Ready() {
            bikePhysics.Progress = GameConfig.GameSettings.currentRoute.startingPoint;

            base._Ready();
        }

        public override void _Process(double delta) {
            distanceFromStart += bikePhysics.speed * (float)delta * speedScale;
            // this shouldn't happen but just to make sure...
            slope =
                (bikePhysics.RotationDegrees.X > 45 ? 100 : 0)
                + Mathf.Tan(bikePhysics.Rotation.X) * 100f;
            slopeAngleRadians = bikePhysics.GlobalRotation.X;

            bikePhysics.Progress = distanceFromStart;

            var rotation = bikePhysics.GlobalRotationDegrees;
            // Z = 0 So the bike doesn't drive on it's side because of path weirdness
            rotation = new(rotation.X, rotation.Y, 0);
            bikePhysics.GlobalRotationDegrees = rotation;
            base._Process(delta);
        }
    }
}
