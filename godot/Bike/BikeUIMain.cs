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
            fpsLabel;

        public override void _Process(double delta) {
            cadenceLabel.Text = $"cadence  : {bikeInput.currentCadence}";
            slopeLabel.Text = $"slope  : {Math.Round((double)bikePhysics.path.slope, 1)}%";
            speedLabel.Text = $"speed 󰓅 : {Math.Round(bikePhysics.speed, 1)}km/h";
            powerLabel.Text = $"power 󱐋 : {bikeInput.currentPower}";
            fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
            base._Process(delta);
        }
    }
}
