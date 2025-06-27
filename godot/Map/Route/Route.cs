namespace ForForm.Map.Route
{
    using Godot;

    [Tool, Icon("res://Script icons/conversion_path.png")]
    public partial class Route : Path3D {
        /// WARN: Remember: when changing or adding any variable add that to RouteExport.cs
        [ExportGroup("Input by hand")]
        [Export]
        public string name;

        [Export(PropertyHint.MultilineText)]
        public string description;

        [Export]
        public Texture2D icon;

        [Export]
        public float startingPoint;

        [Export]
        public string difficulty;

        [Export]
        public uint estimatedTime; // min

        [ExportGroup("Route stats")]
        [Export]
        public float totalDistanceM; // m

        [Export]
        public float ascentM;

        [Export]
        public float descentM;

        [Export]
        public float[] slopeMap; // %

        [Export]
        public float[] heightMapM;

        [Export]
        public float minHeight,
            maxHeight;

        // Checkpoints
        // Connections to other routes


        public void CalculateRouteStats() {
            if (GameConfig.GameSettings.currentMap == null)
                GD.PrintErr("you need to set map before using CalculateRouteStats() ");
            if (GameConfig.GameSettings.currentMap.gameMode == null)
                GD.PrintErr("you need to set game mode before using CalculateRouteStats() ");
            if (GameConfig.GameSettings.currentMap.gameMode.bikeModels[0] == null)
                GD.PrintErr(
                    "you need to set at least 1 bike model before using CalculateRouteStats() "
                );

            float wheelBase = GameConfig
                .GameSettings
                .currentMap
                .gameMode
                .bikeModels[0]
                .modelsWheelBase_m;
            ascentM = 0;
            descentM = 0;

            var points = Curve.GetBakedPoints();
            totalDistanceM = Curve.GetBakedLength() / GameConfig.GameSettings.currentMap.speedScale;

            int pointsLen = points.Length;

            slopeMap = new float[pointsLen];
            heightMapM = new float[pointsLen];

            minHeight = float.MaxValue;
            maxHeight = float.MinValue;

            for (int i = 0; i < pointsLen; i++) {
                float currentProgress = Curve.GetBakedLength() * i / pointsLen;
                var _point = Curve.SampleBaked(currentProgress);

                var frontWhealHeight = Curve.SampleBaked(currentProgress + wheelBase).Y;
                var backWhealHeight = Curve.SampleBaked(currentProgress).Y;
                var heightDelta = frontWhealHeight - backWhealHeight;
                if (heightDelta > 0) {
                    ascentM += heightDelta;
                } else
                    descentM -= heightDelta;
                var angle = Mathf.Atan(heightDelta / wheelBase); // rad
                var slope = Mathf.Tan(angle) * 1; // %

                minHeight = Mathf.Min(minHeight, _point.Y);
                maxHeight = Mathf.Max(maxHeight, _point.Y);
                heightMapM[i] = _point.Y;
                slopeMap[i] = slope;
            }

            ascentM /= GameConfig.GameSettings.currentMap.speedScale;
            descentM /= GameConfig.GameSettings.currentMap.speedScale;
        }
    }
}
