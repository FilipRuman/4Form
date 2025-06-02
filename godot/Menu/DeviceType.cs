using Godot;

namespace ForForm.Menu
{
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
