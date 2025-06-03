using Godot;

namespace ForForm.Menu
{
    public partial class MenuMain : Control {
        [Export]
        PackedScene player;

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
            playerSpawnPoint.AddChild(player.Instantiate());
        }
    }
}
