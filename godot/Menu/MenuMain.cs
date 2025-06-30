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
        internal Map.GameLoader gameLoader;

        [ExportGroup("UI")]
        [Export]
        internal UserConfigMenu userConfigMenu;

        [Export]
        public Game.GameMenu gameMenu;

        [Export]
        TabBar tabBar;

        [Export]
        public MenuTabContent[] tabContents;

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

        public bool gameStarted;

        void HandleLockScreen(MenuTabContent content) {
            bool gameStartedErr = content.gameCantBeStarted && gameStarted;
            tabContentsLockScreen.Visible = gameStartedErr;
            tabContentsLockScreenLabel.Text = (
                gameStartedErr ? "You can't edit contents of this page during active game \n" : ""
            );
        }
    }
}
