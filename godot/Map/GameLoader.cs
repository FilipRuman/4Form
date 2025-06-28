namespace ForForm.Map
{
    using Godot;

    public partial class GameLoader : Node {
        [Export]
        Tcp.TcpParser tcpParser;

        [Export]
        Node3D terrain3D;

        [Export]
        PackedScene playerPrefab;

        public override void _Ready() {
            Bike.BikeStats.userConfig = User.UserConfig.Load();
            base._Ready();
        }

        public void StartGame() {
            GameSettings.gameStarted = true;
            var playerNode = playerPrefab.Instantiate();
            tcpParser.bikePhysics = ((Bike.BikePhysics)playerNode);
            terrain3D.Call("set_camera", ((Bike.BikePhysics)playerNode).camera);
            GameSettings.currentRoute.AddChild(playerNode);
            // terrain 3D is disabled before so it doesn't send irrelevant errors
            terrain3D.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}
