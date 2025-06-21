namespace ForForm.Menu.Game
{
    using System;
    using Godot;

    [Tool]
    public partial class RouteMenu : Control {
        [Export]
        GameMenu gameMenu;

        [Export]
        Control layout;

        [Export]
        PackedScene simpleSelectionPrefab;

        [Export]
        RichTextLabel description;

        [Export]
        Label difficulty,
            time,
            ascent,
            distance;

        public override void _Ready() {
            difficulty.Text = "";
            time.Text = "";
            ascent.Text = "";
            distance.Text = "";
            description.Text = "";
            base._Ready();
        }

        SimpleSelectionUI currentRouteSelectionUI;

        public void Setup(Map.Route.Route[] routes) {
            UIMiscs.ClearChildren(layout);
            foreach (var route in routes) {
                var script = simpleSelectionPrefab.Instantiate() as SimpleSelectionUI;
                layout.AddChild(script);
                if (route == GameConfig.GameSettings.currentRoute)
                    HandleNewSelectionUIHighlight(script);
                script.Setup(
                    route.name,
                    route.icon,
                    () =>
                    {
                        HandleNewSelectionUIHighlight(script);
                        OnRouteSelection(route);
                        gameMenu.OnMenuComplete(1);
                    }
                );
            }
        }

        private void HandleNewSelectionUIHighlight(SimpleSelectionUI newSelectionUI) {
            if (currentRouteSelectionUI != null)
                ThemeVariants.SetForButton(false, currentRouteSelectionUI as Button);
            ThemeVariants.SetForButton(true, newSelectionUI as Button);
            currentRouteSelectionUI = newSelectionUI;
        }

        private void OnRouteSelection(Map.Route.Route route) {
            GameConfig.GameSettings.currentRoute = route;

            description.Text = route.description;
            difficulty.Text = $"Difficulty: {route.difficulty} ";
            time.Text = $"Estimated time to finish: {route.estimatedTime}min ";
            ascent.Text = $"Ascent: {Mathf.RoundToInt(route.ascent)}m ";
            distance.Text = $"Distance: {Math.Round(route.totalDistance / 1000f, 1)}km 󰣰";
        }
    }
}
