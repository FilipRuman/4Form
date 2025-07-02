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

        [Export]
        Animations.UIAnimationPlayer animationPlayer;

        public override void _Process(double delta) {
            base._Process(delta);
            if (Engine.IsEditorHint())
                return;
            if (Input.IsActionJustPressed("ToggleMenu")) {
                if (Visible) {
                    animationPlayer.RunInReverse();
                } else
                    animationPlayer.Run();
            }
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
            tabContents[lastTab].animationPlayer.RunInReverse();
            MenuTabContent currentTab = tabContents[tabBar.CurrentTab];
            currentTab.animationPlayer.Run();
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
