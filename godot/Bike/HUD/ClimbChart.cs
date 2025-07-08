namespace ForForm.Bike.HUD
{
    using System;
    using ForForm.Map.Route;
    using Godot;

    public partial class ClimbChart : TextureRect {
        const int ShaderResolution = 500;

        [Export]
        BikeHudMain main;

        [Export]
        Label minHeight,
            maxHeight,
            distance,
            slope;

        [Export]
        Control playerPositionIndicator;

        [Export]
        Gradient slopeGradient;
        Route Route => main.bikePhysics.route;

        public override void _Ready() {
            UpdateShader();
            UpdateLabels();
            base._Ready();
        }

        // currently not used so when changing routes is implemented just call this to update climb chart nicely
        public void OnRouteChanged() {
            UpdateShader();
            UpdateLabels();
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
            minHeight.Text = $" {((int)(Route.minHeight))}m";
            maxHeight.Text = $" {((int)(Route.maxHeight))}m";
            distance.Text =
                $"{Math.Round(main.bikePhysics.Progress / 1000f, 1)}/{Math.Round(Route.totalDistanceM / 1000f, 1)}km  ";
        }

        private void UpdatePlayerIndicatorPosition() {
            var xPercent = main.bikePhysics.Progress / Route.Curve.GetBakedLength();

            var yPos = Route.heightMap_m[((int)(Route.heightMap_m.Length * xPercent))];
            var yPercent = Mathf.InverseLerp(
                Route.minHeight - height_border_size,
                Route.maxHeight + height_border_size,
                yPos
            );

            playerPositionIndicator.Position = new Vector2(xPercent, 1 - yPercent) * Size;
        }

        // Added so the top and bottom of chart doesn't get clipped + smooths out chart of small maps with low elevation changes
        const float height_border_size = 20;

        private void UpdateShader() {
            var material = ((ShaderMaterial)Material);

            material.SetShaderParameter("points_count", Route.slopeMap.Length);
            material.SetShaderParameter("min_height", Route.minHeight - height_border_size);
            material.SetShaderParameter("max_height", Route.maxHeight + height_border_size);

            material.SetShaderParameter("slope_map", Route.slopeMap);
            material.SetShaderParameter("height_map", Route.heightMap_m);
        }
    }
}
