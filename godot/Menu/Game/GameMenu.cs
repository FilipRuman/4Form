namespace ForForm.Menu.Game
{
    using Godot;

    public partial class GameMenu : Control {
        [Export]
        internal MenuMain menuMain;

        // those refs are setup on _Ready() by those scripts, to make Godot editor less cluttered
        public RouteMenu routeMenu;
        public MapSelectionMenu mapSelectionMenu;
        public BikeConfigurationMenu bikeConfigurationMenu;

        [Export]
        Animations.UIAnimationPlayer[] menus;
        uint currentMenuIndex = 0;

        [Export]
        Button next,
            previous,
            startGame;

        internal void StartGame() {
            menuMain.Visible = false;
            menuMain.gameStarted = true;
            menuMain.gameLoader.StartGame(
                bikeConfigurationMenu.currentBikeModel,
                menuMain.userConfigMenu.userConfig,
                routeMenu.currentRoute,
                mapSelectionMenu.currentMap,
                menuMain
            );
        }

        public override void _Ready() {
            next.Pressed += OnNextMenu;
            next.Visible = false;
            previous.Visible = false;
            previous.Pressed += onPreviousMenu;

            startGame.Pressed += StartGame;
            base._Ready();
        }

        private void OnNextMenu() {
            previous.Visible = true;
            next.Visible = false;
            menus[currentMenuIndex].RunInReverse();
            menus[currentMenuIndex + 1].Run();
            currentMenuIndex++;
        }

        private void onPreviousMenu() {
            next.Visible = false;
            if (currentMenuIndex == 1)
                previous.Visible = false;

            menus[currentMenuIndex].RunInReverse();
            menus[currentMenuIndex - 1].Run();
            currentMenuIndex--;
        }

        public void OnMenuComplete(uint menuIndex) {
            if (currentMenuIndex != menus.Length - 1)
                next.Visible = true;
        }
    }
}
