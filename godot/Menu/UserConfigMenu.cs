namespace ForForm.Menu
{
    using Godot;

    public partial class UserConfigMenu : Control {
        [Export]
        PropertyEdit name,
            mass;

        [Export]
        Button icon;

        [Export]
        MenuMain menuMain;
        private Progression.UserConfig UserConfig => menuMain.progressionManager.userConfig;

        [Export]
        Slider pointsProgress;

        public override void _Ready() {
            SetupPointsProgressSlider();
            name.SetupStr(
                UserConfig.name,
                (text) =>
                {
                    UserConfig.name = text;
                    UserConfig.Save();
                },
                editable: true
            );
            mass.Setup(
                UserConfig.userMass_kg,
                (num) =>
                {
                    UserConfig.userMass_kg = num;
                    UserConfig.Save();
                },
                editable: true
            );

            base._Ready();

            void SetupPointsProgressSlider() {
                pointsProgress.MinValue = 0;
                pointsProgress.MaxValue = 100;
                pointsProgress.Value = menuMain.progressionManager.PointsPercentForNextLevel;
            }
        }
    }
}
