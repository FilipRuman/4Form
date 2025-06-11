namespace ForForm.Menu.DeviceConnection
{
    using Godot;

    [GlobalClass]
    public partial class DeviceType : Node {
        [Export]
        public PeripheralDisplay display;
        public uint peripheralConnectedToIt;

        [Export]
        public string name;

        [Export]
        public string glyphIcon;
    }
}
