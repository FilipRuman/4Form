namespace ForForm.Menu
{
    using System.Collections.Generic;
    using Godot;

    public partial class PeripheralsMenu : Node {
        private HashSet<string> peripherals = new();

        [Export]
        private Tcp.TcpParser tcpParser;

        [Export]
        private PackedScene peripheralDisplayPrefab;

        [Export]
        private DeviceType smartTrainerDeviceType;

        private DeviceType currentDeviceType;

        // I have to use dictionary for this because unknowns are skipped
        private Dictionary<uint, PeripheralDisplay> peripheralDisplaysIndexesDictionary;
        private const string SmartTrainerDeviceTypeName = "SmartTrainer";

        public override void _Ready() {
            smartTrainerDeviceType.display.onClick = () =>
            {
                currentDeviceType = smartTrainerDeviceType;
            };
            base._Ready();
        }

        public void HandlePeripheralsConnection(string name, uint index) {
            if (name == "unknown") {
                return;
            }
            if (peripherals.Add(name)) {
                AddPeripheralDisplay(index, name);
            }
        }

        private void AddPeripheralDisplay(uint index, string name) {
            var peripheralDisplay = peripheralDisplayPrefab.Instantiate() as PeripheralDisplay;
            peripheralDisplaysIndexesDictionary.Add(index, peripheralDisplay);
            peripheralDisplay.Setup(
                onClick: () =>
                {
                    OnPeripheralSelection(index);
                },
                "",
                name,
                false
            );
        }

        private void OnPeripheralSelection(uint index) {
            var deviceTypeName =
                currentDeviceType == smartTrainerDeviceType ? SmartTrainerDeviceTypeName : "";

            tcpParser.SendPeripheralConnectionRequest(index, deviceTypeName);
        }

        private void OnPeripheralConnection(string deviceTypeName, uint index) {
            var glyph = "device type not found!:Peripherals menu 52";
            var deviceType = currentDeviceType;
            switch (deviceTypeName)
            {
                case SmartTrainerDeviceTypeName:
                {
                    glyph = "󰂣 ";
                    deviceType = smartTrainerDeviceType;
                    break;
                }
            }
            ;
            PeripheralDisplay peripheralDisplay = peripheralDisplaysIndexesDictionary[index];
            peripheralDisplay.Highlight();
            peripheralDisplay.icon.Text = glyph;

            deviceType.display.Highlight();
        }

        public class DeviceType {
            [Export]
            public PeripheralDisplay display;
            public uint peripheralConnectedToIt;
        }
    }
}
