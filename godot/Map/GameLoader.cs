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
            Progression.UserConfig userConfig,
            Route.Route route,
            Map map,
            Menu.MenuMain menuMain
        ) {
            var bikePhysics = (Bike.BikePhysics)playerPrefab.Instantiate();

            menuMain.backgroundImage.Visible = false;
            menuMain.SetupOnGameQuitWithWorkout(bikePhysics.hudMain.workout);

            bikePhysics.userMass_kg = userConfig.userMass_kg;
            bikePhysics.userDrag = userConfig.TotalDrag();
            bikePhysics.bikeModel = bikeModel;
            bikePhysics.map = map;
            bikePhysics.route = route;
            bikePhysics.tcpParser = tcpParser;

            tcpParser.bikePhysics = bikePhysics;
            terrain3D.Call("set_camera", bikePhysics.camera);
            route.AddChild(bikePhysics);

            // terrain 3D is disabled before so it doesn't send irrelevant errors
            terrain3D.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}
