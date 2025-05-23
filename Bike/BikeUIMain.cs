namespace ForForm.Bike.UI
{
    using Godot;

    public partial class BikeUIMain : Node {
        [Export]
        BikeInput bikeInput;

        [Export]
        Label cadenceLabel,
            powerLabel,
            fpsLabel;

        public override void _Process(double delta) {
            cadenceLabel.Text = $"cadence: {bikeInput.currentCadence}";
            powerLabel.Text = $"power: {bikeInput.currentPower}";
            fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
            base._Process(delta);
        }
    }
}
