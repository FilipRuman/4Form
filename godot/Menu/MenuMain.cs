namespace ForForm.Menu
{
    using Godot;
    using Tcp;

    public partial class MenuMain : Control {
        [ExportGroup("Outside references")]
        [Export]
        internal Map.WholeMapExport wholeMapExport;

        [Export]
        internal TcpParser tcpParser;

        [Export]
        Map.GameLoader gameLoader;

        [ExportGroup("UI")]
        [Export]
        public Game.GameMenu gameMenu;

        [Export]
        TabBar tabBar;

        [Export]
        public MenuTabContent[] tabContents;

        [Export]
        public Button startGameButton;

        [Export]
        Control tabContentsLockScreen;

        [Export]
        RichTextLabel tabContentsLockScreenLabel;

        public override void _Process(double delta) {
            base._Process(delta);
            if (Engine.IsEditorHint())
                return;
            if (Input.IsActionJustPressed("ToggleMenu"))
                Visible = !Visible;
        }

        public override void _Ready() {
            tabBar.TabChanged += (_) =>
            {
                UpdateTabs();
            };
            startGameButton.Pressed += () =>
            {
                Visible = false;
                gameLoader.StartGame();
            };
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
            bool gameStartedErr = content.gameCantBeStarted && GameSettings.gameStarted;
            bool modeSelectedErr =
                content.gameModeMustBeSelected && GameSettings.CurrentGameMode == null;
            tabContentsLockScreen.Visible = gameStartedErr || modeSelectedErr;
            tabContentsLockScreenLabel.Text =
                (gameStartedErr ? "You can't edit contents of this page during active game \n" : "")
                + (modeSelectedErr ? "You need to select game mode first." : "");
        }
    }
}
