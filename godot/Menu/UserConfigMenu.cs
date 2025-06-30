namespace ForForm.Menu
{
    using Godot;

    public partial class UserConfigMenu : Control {
        [Export]
        PropertyEdit name,
            mass;

        [Export]
        Button icon;
        public UserConfig userConfig;

        public override void _Ready() {
            userConfig = UserConfig.Load();
            name.SetupStr(
                userConfig.name,
                (text) =>
                {
                    userConfig.name = text;
                    userConfig.Save();
                },
                editable: true
            );
            mass.Setup(
                userConfig.mass_kg,
                (num) =>
                {
                    userConfig.mass_kg = num;
                    userConfig.Save();
                },
                editable: true
            );

            base._Ready();
        }
    }
}
