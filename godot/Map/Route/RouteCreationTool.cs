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
        float refreshRateSec = .3f;

        [Export]
        float pathHighlightPointSize = 1;

        [Export]
        public bool highlightPaths;
        float refreshRateTimer = 0;

        public override void _Process(double delta) {
            if (!Engine.IsEditorHint() || !run)
                return;
            if (refreshRateTimer < refreshRateSec) {
                refreshRateTimer += (float)delta;
            }
            refreshRateTimer = 0;
            RunTerrainFollow();
            route.CalculateRouteStats();

            if (highlightPaths)
                ShowHighlight();

            base._Process(delta);
        }

        void ShowHighlight() {
            foreach (var point in roughPath.Curve.GetBakedPoints()) {
                DebugDraw3D.DrawSphere(
                    point,
                    pathHighlightPointSize,
                    Colors.Magenta,
                    refreshRateSec
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
                    slopeColorGradient.Sample(slope01),
                    refreshRateSec
                );
            }
        }

        public void RunTerrainFollow() {
            var points = roughPath.Curve.GetBakedPoints();
            var spaceState = GetWorld3D().DirectSpaceState;
            int length = points.Length;
            // fixes the No target vector when changing bake resolution
            if (outputPath.Curve.PointCount != length) {
                outputPath.Curve.PointCount = length;
                for (int i = 0; i < length; i++) {
                    if (outputPath.Curve.GetPointPosition(i) == Vector3.Zero)
                        outputPath.Curve.SetPointPosition(i, Vector3.One * GD.Randf());
                }
            }

            bool hitAPointInfo = false;
            for (int i = 0; i < length; i++) {
                Vector3 point = points[i];
                Vector3 origin = new(point.X, 10000, point.Z);
                Vector3 end = new(point.X, -10000, point.Z);
                var query = PhysicsRayQueryParameters3D.Create(origin, end);
                var result = spaceState.IntersectRay(query);

                if (!result.TryGetValue("position", out Variant output)) {
                    if (outputPath.Curve.GetPointPosition(reverse ? length - i - 1 : i) == null)
                        outputPath.Curve.SetPointPosition(
                            reverse ? length - i - 1 : i,
                            Vector3.One
                        );

                    continue;
                }
                // hitAPointInfo = true;
                if (
                    outputPath.Curve.GetPointPosition(reverse ? length - i - 1 : i)
                    != (Vector3)output
                )
                    outputPath.Curve.SetPointPosition(
                        reverse ? length - i - 1 : i,
                        (Vector3)output
                    );
            }
            // if (!hitAPointInfo)
            // GD.PrintErr(
            //     "Please, before generating route, set on terrain 3D: Collision>Collision Mode = Full/Editor or Dynamic/Editor "
            // );
        }
    }
}
