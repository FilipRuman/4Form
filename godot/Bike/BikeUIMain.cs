namespace ForForm.Bike.UI
{
    using System;
    using Godot;

    public partial class BikeUIMain : Node {
        [Export]
        BikeInput bikeInput;

        [Export]
        BikePhysics bikePhysics;

        [ExportGroup("UI")]
        [Export]
        Label cadenceLabel,
            powerLabel,
            slopeLabel,
            speedLabel,
            heartRateLabel,
            fpsLabel;

        public override void _Process(double delta) {
            cadenceLabel.Text = $"{bikeInput.currentCadence}rpm cadence ";
            slopeLabel.Text = $"{Math.Round((double)bikePhysics.path.slope, 1)}% slope ";
            speedLabel.Text = $"{Math.Round(bikePhysics.speedKmH, 1)}km/h speed󰓅 ";
            powerLabel.Text = $"{bikeInput.currentPower}w power󱐋";
            heartRateLabel.Text = $"{bikeInput.heartRate}bpm HR ";
            fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
            base._Process(delta);
        }
    }
}
