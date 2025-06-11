namespace ForForm.Menu.GameMode
{
    using System.Collections.Generic;
    using ForForm.GameConfig;
    using Godot;

    [GlobalClass]
    public partial class GameModeMenu : Control {
        [Export]
        Map.MapLoading mapLoading;

        [Export]
        RouteMenu routeMenu;

        [Export]
        Control layout;

        [Export]
        RichTextLabel description;

        [Export]
        PackedScene simpleSelectionPrefab;

        Dictionary<GameMode, SimpleSelectionUI> bikeModelsUI = new();

        //Temp until game loading is implemented

        [Export]
        GameMode[] gameModes;

        public override void _Ready() {
            description.Text = "";
            Setup(gameModes);
            base._Ready();
        }

        public void Setup(GameMode[] gameModes) {
            bikeModelsUI.Clear();
            UIMiscs.ClearChildren(layout);

            foreach (var gm in gameModes) {
                var selectionUI = simpleSelectionPrefab.Instantiate() as SimpleSelectionUI;
                bikeModelsUI.Add(gm, selectionUI);
                layout.AddChild(selectionUI);
                selectionUI.Setup(
                    gm.name,
                    gm.icon,
                    () =>
                    {
                        OnGameModeSelection(gm);
                    }
                );
            }
        }

        private void OnGameModeSelection(GameMode gameMode) {
            if (GameSettings.CurrentGameMode != null) {
                ThemeVariants.SetForButton(
                    highlight: false,
                    bikeModelsUI[GameSettings.CurrentGameMode]
                );
            }
            ThemeVariants.SetForButton(highlight: true, bikeModelsUI[gameMode]);
            description.Text = gameMode.description;
            GameSettings.SetCurrentGameMode(gameMode);
            routeMenu.Setup(mapLoading.Load(gameMode.map));
        }
    }
}
