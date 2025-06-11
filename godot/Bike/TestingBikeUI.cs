namespace ForForm.Bike.UI
{
    using Godot;

    public partial class TestingBikeUI : Node {
        [Export]
        BikePhysics bikePhysics;

        [Export]
        VSlider powerSlider;

        [Export]
        Label powerLabel;

        public override void _Ready() {
            powerLabel.Text = " ";
            powerSlider.ValueChanged += (_) =>
            {
                bikePhysics.testingPower = (float)powerSlider.Value;
                powerLabel.Text = $"Additional power:{(int)bikePhysics.testingPower}W";
            };
            base._Ready();
        }
    }
}
