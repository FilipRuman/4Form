namespace ForForm.Map
{
    using System;
    using Godot;

    public partial class GameLoader : Node {
        [Export]
        Tcp.TcpParser tcpParser;

        [Export]
        Node3D terrain3D;

        [Export]
        PackedScene playerPrefab;

        public void StartGame(
            Bike.BikeModel bikeModel,
            Menu.UserConfig userConfig,
            Route.Route route,
            Map map,
            Menu.MenuMain menuMain
        ) {
            var bikePhysics = (Bike.BikePhysics)playerPrefab.Instantiate();

            menuMain.SetupOnGameQuit(bikePhysics.hudMain.workout);
            bikePhysics.userMass_kg = userConfig.mass_kg;
            bikePhysics.bikeModel = bikeModel;
            bikePhysics.map = map;
            bikePhysics.route = route;

            tcpParser.bikePhysics = bikePhysics;
            terrain3D.Call("set_camera", bikePhysics.camera);
            route.AddChild(bikePhysics);

            // terrain 3D is disabled before so it doesn't send irrelevant errors
            terrain3D.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}
