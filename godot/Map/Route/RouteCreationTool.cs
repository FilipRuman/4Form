namespace ForForm.Map.Route
{
    using Godot;

    [Tool]
    public partial class RouteCreationTool : Node3D {
        [Export]
        public Route route;

        [Export]
        Gradient slopeColorGradient;

        [Export]
        Path3D roughPath;

        [Export]
        Path3D outputPath;

        [Export]
        bool reverse;

        [Export]
        bool run;

        [Export]
        float refreshRate_s = .3f;

        [Export]
        float pathHighlightPointSize = 1;

        [Export]
        public bool highlightPaths;

        [Export]
        Map map;
        float refreshRateTimer = 0;

        public override void _Process(double delta) {
            if (!Engine.IsEditorHint() || !run)
                return;
            if (refreshRateTimer < refreshRate_s) {
                refreshRateTimer += (float)delta;
                return;
            }
            refreshRateTimer = 0;
            RunTerrainFollow();
            if (map == null) {
                GD.PrintErr("you need to set map before using CalculateRouteStats() ");
                refreshRateTimer = -5;
                return;
            }
            route.CalculateRouteStats(map);

            if (highlightPaths)
                HighlightOutputPathPoints();

            base._Process(delta);
        }

        void HighlightOutputPathPoints() {
            foreach (var point in roughPath.Curve.GetBakedPoints()) {
                // show path rough points
                DebugDraw3D.DrawSphere(
                    point,
                    pathHighlightPointSize,
                    Colors.Magenta,
                    // +.01 so they don't flicker
                    refreshRate_s + .01f
                );
            }

            var curve = outputPath.Curve;
            var outputPoints = curve.GetBakedPoints();
            for (int i = 0; i < outputPoints.Length; i++) {
                float currentProgress = curve.GetBakedLength() * i / outputPoints.Length;
                var _point = curve.SampleBaked(currentProgress);

                var slope01 = route.slopeMap[i] / 2f + .5f;
                DebugDraw3D.DrawSphere(
                    _point,
                    pathHighlightPointSize,
                    // Shows slope by color specified in the gradient
                    slopeColorGradient.Sample(slope01),
                    refreshRate_s + .01f
                );
            }
        }

        // uses raycasts to hit terrains collision mesh  that is at the under/over the baked point in rough path
        // places point at the raycast hit point
        public void RunTerrainFollow() {
            var points = roughPath.Curve.GetBakedPoints();
            int length = points.Length;
            // fixes the No target vector when changing bake resolution
            if (outputPath.Curve.PointCount != length) {
                outputPath.Curve.PointCount = length;
                for (int i = 0; i < length; i++) {
                    if (outputPath.Curve.GetPointPosition(i) == Vector3.Zero)
                        outputPath.Curve.SetPointPosition(i, Vector3.One * GD.Randf());
                }
            }

            var spaceState = GetWorld3D().DirectSpaceState;
            for (int i = 0; i < length; i++) {
                Vector3 point = points[i];
                Vector3 origin = new(point.X, 10000, point.Z);
                Vector3 end = new(point.X, -10000, point.Z);
                var query = PhysicsRayQueryParameters3D.Create(origin, end);
                var result = spaceState.IntersectRay(query);

                if (!result.TryGetValue("position", out Variant output))
                    continue;

                if (
                    outputPath.Curve.GetPointPosition(reverse ? length - i - 1 : i)
                    != (Vector3)output
                )
                    outputPath.Curve.SetPointPosition(
                        reverse ? length - i - 1 : i,
                        (Vector3)output
                    );
            }
        }
    }
}
