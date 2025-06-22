namespace ForForm.Menu.Game
{
    using Godot;

[Tool]
    public partial class GameMenu : Control {
        private const int V = 2;
        [Export]
        Control[] menus;
        uint currentMenuIndex = 0;

        [Export]
        Button next,
            previous;

        public override void _Ready() {
            foreach (var menu in menus) {
                menu.Visible = false;
            }
            menus[0].Visible = true;
            next.Pressed += OnNextMenu;
            next.Visible = false;
            previous.Visible = false;
            previous.Pressed += onPreviousMenu;
            base._Ready();
        }

        private void OnNextMenu() {
            previous.Visible = true;
            next.Visible = false;
            menus[currentMenuIndex].Visible = false;
            menus[currentMenuIndex + 1].Visible = true;
            currentMenuIndex++;
        }

        private void onPreviousMenu() {
            next.Visible = false;
            if (currentMenuIndex == 0)
                previous.Visible = false;

            menus[currentMenuIndex].Visible = false;
            menus[currentMenuIndex - 1].Visible = true;
            currentMenuIndex--;
        }

        public void OnMenuComplete(uint menuIndex) {
            if (currentMenuIndex != menus.Length - 1 )
                next.Visible = true;
        }
    }
}
