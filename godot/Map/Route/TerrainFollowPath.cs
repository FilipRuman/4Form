namespace ForForm.Map.Route
{
    using Godot;

    ///HOW TO USE:
    ///1. Create terrain with 3d collider
    ///2. Create path using rough path
    ///3. Control path resolution using: bake interval on the rough path
    ///3. Click run!
    //4. if you turn on the "highlightPaths" you will see rough path baked points(red) and output path baked points(green)
    [Tool]
    public partial class TerrainFollowPath : Node3D {
        [Export]
        Path3D roughPath;

        [Export]
        Path3D outputPath;

        [Export]
        bool run;

        [Export]
        float checkIntervalDistance;

        [ExportGroup("path highlight")]
        [Export]
        float pathHighlightPointSize = 1;

        [Export]
        public bool highlightPaths;

        [Export]
        public float highlightRefreshIntervalSec;

        public override void _Process(double delta) {
            if (!Engine.IsEditorHint())
                return;

            if (run) {
                run = false;
                Run();
            }

            if (highlightPaths)
                ShowHighlight(delta);

            base._Process(delta);
        }

        float highlightRefreshTimer;

        void ShowHighlight(double delta) {
            highlightRefreshTimer += (float)delta;
            if (highlightRefreshTimer < highlightRefreshIntervalSec)
                return;
            highlightRefreshTimer = 0;

            foreach (var point in roughPath.Curve.GetBakedPoints()) {
                DebugDraw3D.DrawSphere(
                    point,
                    pathHighlightPointSize,
                    Colors.Red,
                    highlightRefreshIntervalSec
                );
            }
            foreach (var point in outputPath.Curve.GetBakedPoints()) {
                DebugDraw3D.DrawSphere(
                    point,
                    pathHighlightPointSize,
                    Colors.Green,
                    highlightRefreshIntervalSec
                );
            }
        }

        public void Run() {
            outputPath.Curve.ClearPoints();
            var points = roughPath.Curve.GetBakedPoints();
            GD.Print($"Run {points.Length}");
            var spaceState = GetWorld3D().DirectSpaceState;

            foreach (var point in points) {
                DebugDraw3D.DrawSphere(point, 1, Colors.Red, 10);
                Vector3 origin = new(point.X, 10000, point.Z);
                Vector3 end = new(point.X, -10000, point.Z);
                var query = PhysicsRayQueryParameters3D.Create(origin, end);
                var result = spaceState.IntersectRay(query);

                if (!result.TryGetValue("position", out Variant output))
                    continue;

                DebugDraw3D.DrawSphere((Vector3)output, 1, Colors.Green, 10);

                outputPath.Curve.AddPoint((Vector3)output);
            }
        }
    }
}
