namespace ForForm.Workout
{
    using System.Collections.Generic;
    using System.Linq;
    using Godot;

    public class Workout {
        public float totalTime_s;

        public List<float> wattsList = new();
        public List<float> speedsList = new();
        public List<float> heartRatesList = new();

        public float totalHeartRate;
        public float totalCadence;
        public float totalSpeed_kmH;
        public float totalPower;

        public float averageHeartRate;
        public float averageCadence;
        public float averageSpeed_kmH;
        public float averageWatts;
        public float caloriesBurnt;
        const float humanMetabolicEfficiency = .24f;
        const float julesToCalories = 1f / 4184f;

        public float totalAscent;
        public float totalDescent;

        public float lastHeight;

        const float workoutStatsListsUpdateFrequency_s = 1;
        float workoutStatsListsUpdateTimer;

        public void Update(
            float delta,
            float watts,
            float heartRate,
            float speed_kmH,
            float cadence,
            float currentHeight
        ) {
            workoutStatsListsUpdateTimer += delta;
            if (workoutStatsListsUpdateTimer > workoutStatsListsUpdateFrequency_s) {
                workoutStatsListsUpdateTimer = 0;

                wattsList.Add(watts);
                heartRatesList.Add(watts);
                speedsList.Add(speed_kmH);
            }
            totalTime_s += delta;

            totalPower += watts * delta;
            totalCadence += cadence * delta;
            totalHeartRate += heartRate * delta;
            totalSpeed_kmH += speed_kmH * delta;

            float deltaHeight = currentHeight - lastHeight;
            if (deltaHeight > 0)
                totalAscent += deltaHeight; else
                totalDescent += deltaHeight;
            lastHeight = currentHeight;

            averageCadence = totalCadence / totalTime_s;
            averageHeartRate = totalHeartRate / totalTime_s;
            averageSpeed_kmH = totalSpeed_kmH / totalTime_s;
            averageWatts = totalPower / totalTime_s;

            caloriesBurnt = averageWatts * totalTime_s * julesToCalories / humanMetabolicEfficiency;
        }

        public static Workout Load(string name) {
            var file = FileAccess.Open(
                $"user://Workouts/{name}/data.json",
                FileAccess.ModeFlags.Read
            );

            var data = ((Godot.Collections.Dictionary)Json.ParseString(file.GetAsText()));

            return new Workout() {
                wattsList = GD.StrToVar(data["wattsList"].ToString()).AsFloat32Array().ToList(),
                heartRatesList = GD.StrToVar(data["heartRatesList"].ToString())
                    .AsFloat32Array()
                    .ToList(),
                speedsList = GD.StrToVar(data["speedsList"].ToString()).AsFloat32Array().ToList(),

                totalAscent = ((float)data["totalAscent"]),
                totalDescent = ((float)data["totalDescent"]),
                averageCadence = ((float)data["averageCadence"]),
                averageHeartRate = ((float)data["averageHeartRate"]),
                averageSpeed_kmH = ((float)data["averageSpeed_kmH"]),
                averageWatts = ((float)data["averageWatts"]),
                caloriesBurnt = ((float)data["caloriesBurnt"]),
            };
        }

        public void Save() {
            DirAccess.Open($"user://").MakeDir("Workouts");
            DirAccess.Open($"user://Workouts/").MakeDir($"{Time.GetDateStringFromSystem()}");

            string _basePath = $"user://Workouts/{Time.GetDateStringFromSystem()}/";
            var data = new Godot.Collections.Dictionary {
                { "wattsList", GD.VarToStr(wattsList.ToArray()) },
                { "heartRatesList", GD.VarToStr(heartRatesList.ToArray()) },
                { "speedsList", GD.VarToStr(speedsList.ToArray()) },
                { "totalAscent", totalAscent },
                { "totalDescent", totalDescent },
                { "averageCadence", averageCadence },
                { "averageHeartRate", averageHeartRate },
                { "averageSpeed_kmH", averageSpeed_kmH },
                { "averageWatts", averageWatts },
                { "caloriesBurnt", caloriesBurnt },
            };
            var text = Json.Stringify(data, "\t");

            var file = FileAccess.Open(_basePath + "data.json", FileAccess.ModeFlags.Write);
            file.StoreLine(text);
            file.Flush();

            // Calling dispose is needed because otherwise ***.json.temp02*** instead of normal json files are created
            // idk. this sometimes spawns temp files anyway....
            file.Dispose();
        }
    }
}
