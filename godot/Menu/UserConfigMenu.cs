namespace ForForm.Menu
{
    using Godot;

    public partial class UserConfigMenu : Control {
        [Export]
        PropertyEdit name,
            mass;

        [Export]
        Button icon;

        public override void _Ready() {
            Bike.BikeStats.userConfig =  Player.UserConfig.Load();
GD.Print($"Bike.BikeStats.userConfig.name {Bike.BikeStats.userConfig.name}");
            name.SetupStr(
                Bike.BikeStats.userConfig.name,
                (text) =>
                {
                    Bike.BikeStats.userConfig.name = text;
                    Bike.BikeStats.userConfig.Save();
                },
                editable: true
            );
            mass.Setup(
                Bike.BikeStats.userConfig.mass,
                (num) =>
                {
                    Bike.BikeStats.userConfig.mass = num;
                    Bike.BikeStats.userConfig.Save();
                },
                editable: true
            );

            base._Ready();
        }
    }
}
