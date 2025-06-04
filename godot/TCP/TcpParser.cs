namespace ForForm.Tcp
{
    using System.Text.RegularExpressions;
    using Godot;

    public partial class TcpParser : Node {
        [Export]
        Tcp tcp;

        [Export]
        Menu.PeripheralsMenu peripheralsMenu;
        public Bike.BikePhysics bikePhysics;
        RegEx standardIndexNameRegex = new RegEx();
        RegEx trainerDataRegex = new RegEx();

        public override void _Ready() {
            trainerDataRegex.Compile(
                """power:(?<power>\d*);cadence:(?<cadence>\d*);rotation:(?<rotation>\d*);"""
            );
            standardIndexNameRegex.Compile("""\|(?<name>.*)\|\[(?<index>.*)\]""");
            base._Ready();
        }

        public void ParseTcpDataString(string data) {
            // c# switch statements are UGLY compered to rust...
            GD.Print($"ParseTcpDataString '{data}'");
            switch (data[0])
            {
                case 't':
                {
                    //"t power:477;cadence:321;rotation:123;"
                    if (bikePhysics == null)
                        return;
                    var regexOutput = trainerDataRegex.Search(data[1..data.Length]);
                    bikePhysics.input.currentPower = uint.Parse(regexOutput.GetString("power"));
                    bikePhysics.input.currentCadence = uint.Parse(regexOutput.GetString("cadence"));
                    bikePhysics.input.wheelRotation = uint.Parse(regexOutput.GetString("rotation"));

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
