namespace ForForm.Bike.HUD
{
    using Godot;

    public partial class TestingBikeHUD : Node {
        [Export]
        BikeHUDMain main;

        [Export]
        VSlider powerSlider;

        [Export]
        Label powerLabel;

        public override void _Ready() {
            powerLabel.Text = " ";
            powerSlider.ValueChanged += (_) =>
            {
                main.bikePhysics.testingPower = (float)powerSlider.Value;
                powerLabel.Text = $"Additional power:{(int)main.bikePhysics.testingPower}W";
            };
            base._Ready();
        }
    }
}
