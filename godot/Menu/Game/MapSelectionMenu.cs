namespace ForForm.Menu.Game
{
    using Godot;
    using Map;

    public partial class MapSelectionMenu : Control {
        [Export]
        Control localLayout,
            onlineLayout;

        [Export]
        RichTextLabel description;

        [Export]
        GameMenu gameMenu;
        internal Map currentMap;

        public override void _Ready() {
            gameMenu.mapSelectionMenu = this;
            Miscs.ClearChildren(localLayout);
            Miscs.ClearChildren(onlineLayout);
            description.Text = "";
            DisplayMaps();
            base._Ready();
        }

        public void DisplayMaps() {
            LocalMaps();
            OnlineMaps();
        }

        Button lastMapButtonSelected;

        private void LocalMaps() {
            if (DirAccess.Open("user://Maps/") == null)
                return;
            var maps = DirAccess.Open("user://Maps/").GetDirectories();
            foreach (var mapName in maps) {
                var button = new Button() { Text = mapName };
                button.AddThemeColorOverride("font_color", Color.Color8(255, 255, 255, 255));

                button.Pressed += () =>
                {
                    if (lastMapButtonSelected != null)
                        ThemeVariants.SetForButton(false, lastMapButtonSelected);

                    currentMap = gameMenu.menuMain.wholeMapExport.Import(mapName);

                    lastMapButtonSelected = button;
                    ThemeVariants.SetForButton(true, button);
                    description.Text = currentMap.description;

                    gameMenu.OnMenuComplete(menuIndex: 0);

                    gameMenu.bikeConfigurationMenu.OnNewMap(currentMap);
                    gameMenu.routeMenu.Setup(currentMap.routes);
                };

                ThemeVariants.SetForButton(false, button);
                localLayout.AddChild(button);
            }
        }

        private void OnlineMaps() {
            // TODO: Add with multiplayer release
        }
    }
}
