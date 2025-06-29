namespace ForForm.Bike.HUD
{
    using System;
    using ForForm.Map.Route;
    using Godot;

    public partial class ClimbChart : TextureRect {
        const int ShaderResolution = 500;

        [Export]
        BikeHUDMain main;

        [Export]
        Label minHeight,
            maxHeight,
            distance,
            slope;

        [Export]
        Control playerPositionIndicator;

        [Export]
        Gradient slopeGradient;

        public override void _Ready() {
            UpdateShader(GameSettings.currentRoute);
            UpdateLabels();

            GameSettings.onCurrentRouteChanged += () =>
            {
                UpdateShader(GameSettings.currentRoute);
                UpdateLabels();
            };
            base._Ready();
        }

        public override void _Process(double delta) {
            UpdatePlayerIndicatorPosition();
            slope.AddThemeColorOverride(
                "font_color",
                slopeGradient.Sample(main.bikePhysics.slope_percent / 200f + .5f)
            );
            slope.Text = $"{Math.Round((double)main.bikePhysics.slope_percent, 1)}% slope ";
            base._Process(delta);
        }

        private void UpdateLabels() {
            Route currentRoute = GameSettings.currentRoute;

            minHeight.Text = $" {((int)(currentRoute.minHeight))}m";
            maxHeight.Text = $" {((int)(currentRoute.maxHeight))}m";
            distance.Text = $"{Math.Round(currentRoute.totalDistanceM / 1000f, 1)}km  ";
        }

        private void UpdatePlayerIndicatorPosition() {
            Route currentRoute = GameSettings.currentRoute;
            var xPercent = main.bikePhysics.Progress / currentRoute.Curve.GetBakedLength();

            var yPos = currentRoute.heightMapM[((int)(currentRoute.heightMapM.Length * xPercent))];
            var yPercent = Mathf.InverseLerp(currentRoute.minHeight, currentRoute.maxHeight, yPos);

            playerPositionIndicator.Position = new Vector2(xPercent, 1 - yPercent) * Size;
        }

        private void UpdateShader(Route route) {
            var material = ((ShaderMaterial)Material);

            material.SetShaderParameter("points_count", route.slopeMap.Length);
            material.SetShaderParameter("min_height", route.minHeight);
            material.SetShaderParameter("max_height", route.maxHeight);

            material.SetShaderParameter("slope_map", route.slopeMap);
            material.SetShaderParameter("height_map", route.heightMapM);
        }
    }
}
