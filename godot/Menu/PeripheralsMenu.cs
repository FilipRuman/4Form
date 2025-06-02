namespace ForForm.Menu
{
    using System.Collections.Generic;
    using Godot;

    public partial class PeripheralsMenu : Node {
        private HashSet<string> peripherals = new();

        [Export]
        private Control peripheralsDisplaysLayout;

        [Export]
        private Tcp.TcpParser tcpParser;

        [Export]
        private PackedScene peripheralDisplayPrefab;
        private DeviceType currentDeviceType;

        [Export]
        private DeviceType[] deviceTypes;

        // I have to use dictionary for this because unknowns are skipped
        private Dictionary<uint, PeripheralDisplay> peripheralDisplaysIndexesDictionary = new();

        public override void _Ready() {
            foreach (var _deviceType in deviceTypes) {
                _deviceType.display.onClick = () =>
                {
                    TurnOffAllDeviceTypeSelection();

                    _deviceType.display.ButtonPressed = true;
                    currentDeviceType = _deviceType;
                };
            }

            base._Ready();
        }

        private void TurnOffAllDeviceTypeSelection() {
            foreach (DeviceType deviceType in deviceTypes) {
                deviceType.display.ButtonPressed = false;
            }
        }

        public void DisplayNewPeripheral(string name, uint index) {
            if (name == "unknown") {
                return;
            }
            if (peripherals.Add(name)) {
                AddPeripheralDisplay(index, name);
            }
        }

        private void AddPeripheralDisplay(uint index, string name) {
            var peripheralDisplay = peripheralDisplayPrefab.Instantiate() as PeripheralDisplay;
            peripheralsDisplaysLayout.AddChild(peripheralDisplay);

            peripheralDisplaysIndexesDictionary.Add(index, peripheralDisplay);
            peripheralDisplay.Setup(
                _onClick: () =>
                {
                    OnPeripheralSelection(index);
                },
                "",
                name,
                false
            );
        }

        private void OnPeripheralSelection(uint index) {
            var deviceTypeName = currentDeviceType.name;

            tcpParser.SendPeripheralConnectionRequest(index, deviceTypeName);
        }

        public void OnPeripheralConnection(string deviceTypeName, uint index) {
            var glyph = "device type not found!:Peripherals menu 52";
            var matchingDeviceType = currentDeviceType;
            // switch or dictionary would be faster but this  is cleaner
            foreach (var _deviceType in deviceTypes) {
                if (_deviceType.name != deviceTypeName)
                    continue;
                matchingDeviceType = _deviceType;
                glyph = _deviceType.glyphIcon;
            }

            PeripheralDisplay peripheralDisplay = peripheralDisplaysIndexesDictionary[index];
            peripheralDisplay.Highlight(true);
            peripheralDisplay.icon.Text = glyph;

            matchingDeviceType.display.Highlight(true);
        }

    }
}
