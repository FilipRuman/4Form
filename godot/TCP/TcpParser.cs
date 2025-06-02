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
        RegEx peripheralsNameRegex = new RegEx();
        RegEx peripheralsIndexRegex = new RegEx();

        public override void _Ready() {
            // match all symbols inside []
            peripheralsNameRegex.Compile("\\[.*\\]");
            // match all symbols inside ||
            peripheralsIndexRegex.Compile("\\|.*\\|");
            base._Ready();
        }

        public void ParseTcpDataString(string data) {
            // c# switch statements are UGLY compered to rust...
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
                    var nameRough = peripheralsNameRegex.Search(data[1..data.Length]).Strings[0];

                    var indexRough = peripheralsIndexRegex.Search(data[1..data.Length]).Strings[0];

                    peripheralsMenu.HandlePeripheralsConnection(
                        nameRough[1..(nameRough.Length - 1)],
                        uint.Parse(indexRough[1..(indexRough.Length - 1)])
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
