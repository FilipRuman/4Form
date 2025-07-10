namespace ForForm.Progression
{
    using Godot;

    public partial class ProgressionManager : Node {
        [Export]
        private Curve pointsForNextLevelCurve;
        public UserConfig userConfig;
        public float PointsPercentForNextLevel =>
            (userConfig.points / pointsForNextLevelCurve.SampleBaked(userConfig.level)) * 100;

        [Export]
        float pointsModifier;

        public override void _Ready() {
            userConfig = UserConfig.Load();
            base._Ready();
        }

        public void AddPointsFromWorkout(Workout.Workout workout) {
            userConfig.points += pointsModifier * workout.totalPower / userConfig.userMass_kg;
            float pointsForNextLevel = pointsForNextLevelCurve.SampleBaked(userConfig.level);

            if (userConfig.points > pointsForNextLevel) {
                userConfig.level++;
                userConfig.points -= pointsForNextLevel;
            }

            userConfig.Save();
        }
    }
}
