namespace ForForm.Menu.Game
{
    using Godot;

    public partial class MapSelectionMenu : Control {
        [Export]
        Control localLayout,
            onlineLayout;

        [Export]
        RichTextLabel description;

        [Export]
        Map.WholeMapExport wholeMapExport;

        [Export]
        GameMenu gameMenu;

        public override void _Ready() {
            GameConfig.GameSettings.onCurrentGameModeChanged += () =>
            {
                description.Text = GameConfig.GameSettings.currentMap.description;
            };
            UIMiscs.ClearChildren(localLayout);
            UIMiscs.ClearChildren(onlineLayout);
            description.Text = "";
            DisplayMaps();
            base._Ready();
        }

        public void DisplayMaps() {
            LocalMaps();
            OnlineMaps();
        }

        Button lastMapSelected;

        private void LocalMaps() {
            var maps = DirAccess.Open("user://Maps/").GetDirectories();
            foreach (var mapName in maps) {
                var button = new Button() { Text = mapName };

                button.Pressed += () =>
                {
                    if (lastMapSelected != null)
                        ThemeVariants.SetForButton(false, lastMapSelected);
                    lastMapSelected = button;

                    ThemeVariants.SetForButton(true, button);
                    wholeMapExport.Import(mapName);
                    gameMenu.OnMenuComplete(menuIndex: 0);
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
