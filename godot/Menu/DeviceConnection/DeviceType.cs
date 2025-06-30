namespace ForForm.Menu.DeviceConnection
{
    using Godot;

    [GlobalClass]
    public partial class DeviceType : Node {
        [Export]
        public PeripheralDisplay display;
        public uint peripheralConnectedToIt;

        /// needs to exactly match device type name that is specified in rust side tcp parser:
        [Export]
        public string bluetoothDeviceTypeName;

        [Export]
        public string glyphIcon;
    }
}
