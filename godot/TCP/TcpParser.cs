namespace ForForm.Tcp
{
    using System.Text.RegularExpressions;
    using Godot;

    public partial class TcpParser : Node {
        [Export]
        Tcp tcp;

        [Export]
        Menu.PeripheralsMenu peripheralsMenu;

        [Export]
        Bike.BikeInput bikeInput;
        RegEx standardIndexNameRegex = new RegEx();

        public override void _Ready() {
            standardIndexNameRegex.Compile("""\|(?<name>.*)\|\[(?<index>.*)\]""");
            base._Ready();
        }

        public void ParseTcpDataString(string data) {
            // c# switch statements are UGLY compered to rust...
            GD.Print($"ParseTcpDataString '{data}'");
            switch (data[0])
            {
                case 'c':
                {
                    uint.TryParse(data[1..data.Length], out bikeInput.currentCadence);
                    break;
                }
                case 'p':
                {
                    uint.TryParse(data[1..data.Length], out bikeInput.currentPower);
                    break;
                }
                case 'i':
                {
                    var regexOutput = standardIndexNameRegex.Search(data[1..data.Length]);
                    peripheralsMenu.DisplayNewPeripheral(
                        regexOutput.GetString("name"),
                        uint.Parse(regexOutput.GetString("index"))
                    );
                    break;
                }
                case 'o':
                {
                    var regexOutput = standardIndexNameRegex.Search(data[1..data.Length]);
                    peripheralsMenu.OnPeripheralConnection(
                        regexOutput.GetString("name"),
                        uint.Parse(regexOutput.GetString("index"))
                    );

                    break;
                }
            }
        }

        public void SendPeripheralConnectionRequest(uint index, string deviceTypeName) {
            tcp.SendDataAsync($"i|{deviceTypeName}|[{index}]");
        }
    }
}
