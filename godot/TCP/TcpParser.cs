namespace ForForm.Tcp
{
    using System.Text.RegularExpressions;
    using Godot;

    public partial class TcpParser : Node {
        [Export]
        public Tcp tcp;

        [Export]
        Menu.MenuMain menuMain;
        public Bike.BikePhysics bikePhysics;
        RegEx standardIndexNameRegex = new RegEx();
        RegEx trainerDataRegex = new RegEx();

        RegEx heartRateDataRegex = new RegEx();

        public void Setup() {
            trainerDataRegex.Compile(
                """power:(?<power>\d*);cadence:(?<cadence>\d*);rotation:(?<rotation>\d*);"""
            );
            heartRateDataRegex.Compile("""hr:(?<hr>\d*);""");
            standardIndexNameRegex.Compile("""\|(?<name>.*)\|\[(?<index>.*)\]""");
            base._Ready();
        }

        [Export]
        bool debugTCPData;

        public void ParseTcpDataString(string data) {
            // c# switch statements are UGLY compered to rust...
            if (debugTCPData)
                GD.Print($"ParseTcpDataString '{data}'");
            switch (data[0])
            {
                case 't':
                {
                    //"t power:477;cadence:321;rotation:123;"
                    if (bikePhysics == null)
                        return;
                    var regexOutput = trainerDataRegex.Search(data[1..data.Length]);
                    bikePhysics.input.currentWatts = uint.Parse(regexOutput.GetString("power"));
                    bikePhysics.input.currentCadence_RPM = uint.Parse(
                        regexOutput.GetString("cadence")
                    );
                    bikePhysics.input.wheelRotation_degS = uint.Parse(
                        regexOutput.GetString("rotation")
                    );

                    break;
                }
                case 'i':
                {
                    var regexOutput = standardIndexNameRegex.Search(data[1..data.Length]);
                    menuMain.peripheralsMenu.DisplayNewPeripheral(
                        regexOutput.GetString("name"),
                        uint.Parse(regexOutput.GetString("index"))
                    );
                    break;
                }
                case 'o':
                {
                    var regexOutput = standardIndexNameRegex.Search(data[1..data.Length]);
                    menuMain.peripheralsMenu.OnPeripheralConnection(
                        regexOutput.GetString("name"),
                        uint.Parse(regexOutput.GetString("index"))
                    );

                    break;
                }
                case 'h':
                {
                    var regexOutput = heartRateDataRegex.Search(data[1..data.Length]);
                    bikePhysics.input.heartRate = uint.Parse(regexOutput.GetString("hr"));

                    break;
                }
            }
        }

        public void SendPeripheralConnectionRequest(uint index, string deviceTypeName) {
            tcp.SendDataAsync($"i|{deviceTypeName}|[{index}]");
        }
    }
}
