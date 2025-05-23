namespace ForForm.Tcp
{
    using Godot;

    public partial class TcpParser : Node {
        [Export]
        Bike.BikeInput bikeInput;

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
            }
        }
    }
}
