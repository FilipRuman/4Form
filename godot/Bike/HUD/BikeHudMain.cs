namespace ForForm.Bike.HUD
{
    using Godot;

    public partial class BikeHUDMain : CanvasLayer {
        //INFO:  I put all references in one script so there is less clutter in the godot editor
        //  Also it makes setting separate scenes for each segment of code easier
        [Export]
        internal BikeInput bikeInput;

        [Export]
        internal BikePhysics bikePhysics;

        [ExportGroup("UI")]
        [Export]
        Label cadence,
            cadenceAvr,
            power,
            powerAvr,
            speed,
            speedAvr,
            heartRate,
            heartRateAvr,
            workoutLength,
            totalDescent,
            caloriesBurnt,
            totalAscent,
            fps;

        public Workout.Workout workout = new();

        public override void _Process(double delta) {
            if (Input.IsActionJustPressed("HideHUD")) {
                Visible = !Visible;
            }
            workout.Update(
                (float)delta,
                bikeInput.currentWatts,
                bikeInput.heartRate,
                bikePhysics.speed_kmH,
                bikeInput.currentCadence_RPM,
                bikePhysics.GlobalPosition.Y
            );

            workoutLength.Text = $"{(int)workout.totalTime_s / 60}";
            totalDescent.Text = $"{(int)workout.totalDescent}";
            totalAscent.Text = $"{(int)workout.totalAscent}";
            caloriesBurnt.Text = $"{(int)workout.caloriesBurnt}󰆘";

            cadence.Text = $" {bikeInput.currentCadence_RPM}";
            cadenceAvr.Text = $"{workout.averageCadence}";

            speed.Text = $"{(int)bikePhysics.speed_kmH}󰓅";
            speedAvr.Text = $"{(int)workout.averageSpeed_kmH}";

            power.Text = $"󱐋 {bikeInput.currentWatts}";
            powerAvr.Text = $"{(int)workout.averageWatts}";

            heartRate.Text = $"{bikeInput.heartRate}";
            heartRateAvr.Text = $"{workout.averageHeartRate}";

            fps.Text = $"FPS: {Engine.GetFramesPerSecond()}";
            base._Process(delta);
        }
    }
}
