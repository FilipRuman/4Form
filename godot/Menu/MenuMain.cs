namespace ForForm.Menu
{
    using Godot;
    using Tcp;

    public partial class MenuMain : Control {
        [Export]
        TcpParser tcpParser;

        [Export]
        PackedScene playerPrefab;

        [Export]
        TabBar tabBar;

        [Export]
        public MenuTabContent[] tabContents;

        [Export]
        public Button startGameButton;

        [Export]
        Control tabContentsLockScreen;
[Export]
Node3D terrain3D;
        [Export]
        RichTextLabel tabContentsLockScreenLabel;

        public override void _Process(double delta) {
            base._Process(delta);
            if (Input.IsActionJustPressed("ToggleMenu"))
                Visible = !Visible;
        }

        public override void _Ready() {
            tabBar.TabChanged += (_) =>
            {
                UpdateTabs();
            };
            startGameButton.Pressed += StartGame;
            base._Ready();
        }

        int lastTab = 0;

        public void UpdateTabs() {
            if (lastTab == tabBar.CurrentTab)
                return;
            tabContents[lastTab].Visible = false;
            MenuTabContent currentTab = tabContents[tabBar.CurrentTab];
            currentTab.Visible = true;
            HandleLockScreen(currentTab);
            lastTab = tabBar.CurrentTab;
        }

        void HandleLockScreen(MenuTabContent content) {
            bool gameStartedErr = content.gameCantBeStarted && GameConfig.GameSettings.gameStarted;
            bool modeSelectedErr =
                content.gameModeMustBeSelected && GameConfig.GameSettings.CurrentGameMode == null;
            tabContentsLockScreen.Visible = gameStartedErr || modeSelectedErr;
            tabContentsLockScreenLabel.Text =
                (gameStartedErr ? "You can't edit contents of this page during active game \n" : "")
                + (modeSelectedErr ? "You need to select game mode first." : "");
        }
// TODO: move this to better place
        public void StartGame() {
            Visible = false;
            GameConfig.GameSettings.gameStarted = true;
            var playerNode = playerPrefab.Instantiate();
            tcpParser.bikePhysics = ((Bike.BikePhysics)playerNode);
            terrain3D.Call(
                "set_camera",
                ((Bike.BikePhysics)playerNode).camera
            );
            GameConfig.GameSettings.currentRoute.AddChild(playerNode);
        }
    }
}
