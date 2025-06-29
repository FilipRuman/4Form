namespace ForForm.Bike.HUD
{
    using Godot;

    public partial class BikeHUDMain : Node {
        //INFO:  I put all references in one script so there is less clutter in the godot editor
        //  Also it makes setting separate scenes for each segment of code easier
        [Export]
        internal BikeInput bikeInput;

        [Export]
        internal BikePhysics bikePhysics;

        [ExportGroup("UI")]
        [Export]
        Label cadence,
            power,
            speed,
            heartRate,
            fps;

        public override void _Process(double delta) {
            cadence.Text = $"{bikeInput.currentCadence_RPM}rpm cadence ";

            speed.Text = $"{(int)bikePhysics.speed_kmH}km/h speed󰓅 ";
            power.Text = $"{bikeInput.currentWatts}w power󱐋";
            heartRate.Text = $"{bikeInput.heartRate}bpm HR ";
            fps.Text = $"FPS: {Engine.GetFramesPerSecond()}";
            base._Process(delta);
        }
    }
}
