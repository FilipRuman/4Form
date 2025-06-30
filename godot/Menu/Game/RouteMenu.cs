namespace ForForm.Menu.Game
{
    using System;
    using Godot;
    using Map.Route;

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
            gameMenu.routeMenu = this;
            difficulty.Text = "";
            time.Text = "";
            ascent.Text = "";
            distance.Text = "";
            // this was causing weird issues
            if (description != null)
                description.Text = "";
            base._Ready();
        }

        SimpleSelectionUI currentRouteSelectionUI;
        internal Route currentRoute;

        public void Setup(Route[] routes) {
            Miscs.ClearChildren(layout);

            foreach (var route in routes) {
                var script = simpleSelectionPrefab.Instantiate() as SimpleSelectionUI;
                layout.AddChild(script);
                if (route == currentRoute)
                    HandleNewSelectionUIHighlight(script);
                script.Setup(
                    route.name,
                    route.icon,
                    () =>
                    {
                        HandleNewSelectionUIHighlight(script);
                        OnRouteSelection(route);
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

        private void OnRouteSelection(Route route) {
            currentRoute = route;
            gameMenu.OnMenuComplete(1);

            description.Text = route.description;
            difficulty.Text = $"Difficulty: {route.difficulty} ";
            time.Text = $"Estimated time to finish: {route.estimatedTime}min ";
            ascent.Text = $"Ascent: {Mathf.RoundToInt(route.ascentM)}m ";
            distance.Text = $"Distance: {Math.Round(route.totalDistanceM / 1000f, 1)}km 󰣰";
        }
    }
}
