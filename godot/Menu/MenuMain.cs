
namespace ForForm.Menu
{

using Godot;
	using Tcp; 
    public partial class MenuMain : Control {
	    [Export]TcpParser tcpParser;
        [Export]
        PackedScene playerPrefab;

        [Export]
        Node3D playerSpawnPoint;

        [Export]
        TabBar tabBar;

        [Export]
        public Control[] tabContents;

        [Export]
        public Button startGameButton;

        public override void _Process(double delta) {
            base._Process(delta);
            UpdateTabs();
        }

        public override void _Ready() {
            startGameButton.Pressed += StartGame;
            base._Ready();
        }

        int lastTab = 0;

        public void UpdateTabs() {
            if (lastTab == tabBar.CurrentTab)
                return;
            tabContents[lastTab].Visible = false;
            tabContents[tabBar.CurrentTab].Visible = true;

            lastTab = tabBar.CurrentTab;
        }

        public void StartGame() {
            Visible = false;
	    var playerNode = playerPrefab.Instantiate();
	    tcpParser.bikePhysics = ((Bike.BikePhysics) playerNode  );
            playerSpawnPoint.AddChild(playerNode);
        }
    }
}
