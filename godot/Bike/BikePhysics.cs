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
        [Export]
        BikePath path;

        [Export]
        public BikeInput input;

        [Export]
        BikeStats stats;

        [Export]
        public float speed; //m/s
        public float speedKmH => speed * 3.6f; // km/h

        [Export]
        float gravity; //m/s^2
        float gravityForce => stats.totalMass * gravity; //N
        float slopeGravityForce => gravityForce * Mathf.Sin(path.slopeAngleRadians); //the force that is pushing you forward from hills

        // If you take a corner that looks like nascar track (curved to the inside) the we would need to account other forces but this doesn't matter
        float normalGravityForce => gravityForce * Mathf.Cos(path.slopeAngleRadians); //the force that is applied directly to the ground
        float rollingResistanceForce =>
            stats.wheelFrictionCoefficient * normalGravityForce * float.Sign(speed); //N

        // I could use formula from my flight sim XD https://github.com/FilipRuman/Flight-sim
        const float StandardAirDensity = 1.2250f; // kg/m^3
        float airDragForce =>
            stats.frontalArea
            * stats.dragCoefficient
            * StandardAirDensity
            * Mathf.Pow(speed, 2)
            / 2f
            * float.Sign(speed); //N

        float DegSToRadS =  Mathf.Pi / 180f;

        //https://en.wikipedia.org/wiki/Torque
        // TODO: use data from trainer for this
        float whealAngularVelocity => input.wheelRotation * DegSToRadS; // rad/s

        // this is pushing straight forward so we can replace dot product by multiplication
        float torque => whealAngularVelocity == 0 ? 0 : input.currentPower / whealAngularVelocity; // Nm

        // this is pushing straight forward so we can replace cross product by multiplication
        float drivetrainForwardPushingForce => torque / stats.wheelRadius; // N

        float totalForwardForce =>
            drivetrainForwardPushingForce
            - slopeGravityForce
            - rollingResistanceForce
            - airDragForce; //N
        public float acceleration => totalForwardForce / stats.totalMass; // m/s^2 clamped to remove any weirdness

        public override void _Process(double delta) {
            GD.Print($"drivetrainForwardPushingForce {drivetrainForwardPushingForce}: {whealAngularVelocity} {torque} {input.currentPower}");
            // so you don't roll backwards on hills when stopping pedaling
            speed = Mathf.Max(speed + acceleration * (float)delta, 0);
            base._Process(delta);
        }
    }
}
