namespace ForForm.Bike
{
	using Godot;

	public partial class BikePath : Node {
		[Export]
		BikePhysics bikePhysics;

		[Export]
		PathFollow3D pathFollow;
		float distanceFromStart;

		public float slope; // percentage -> 0deg = 0.0 - flat, 90deg = 1.0 - straight up, -20 deg = 0.(2)
		public float slopeAngleRadians;

		public override void _Process(double delta) {
			distanceFromStart += bikePhysics.speed * (float)delta;

			slope = bikePhysics.GlobalRotationDegrees.X / 90f;
			slopeAngleRadians = bikePhysics.GlobalRotation.X;

			bikePhysics.Progress = distanceFromStart;
			base._Process(delta);
		}
	}
}
