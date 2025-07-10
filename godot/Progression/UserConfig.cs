namespace ForForm.Progression
{
    using System;
    using System.Collections.Generic;
    using Godot;

    public class UserConfig {
        public string name = "";

        public uint level,
            money;
        public float points;

        public Godot.Collections.Dictionary<Wearable.Type, Wearable> outfit = new();

        public float userMass_kg;

        public const string Path = "user://UserConfig.json";

        public void GetTotalStats(
            Curve statsChangeWithWearCurve,
            out float totalMass_kg,
            out float totalAirDrag
        ) {
            totalMass_kg = 0;
            totalAirDrag = 0;
            foreach (Wearable.Type wearableType in Enum.GetValues(typeof(Wearable.Type))) {
                var wearable = outfit[wearableType];
                if (wearable == null) {
                    GD.PrintErr(
                        $"there is no wearable of type{wearableType} in the outfit dictionary!"
                    );
                }
                var statsModifier = statsChangeWithWearCurve.SampleBaked(wearable.wear);
                totalAirDrag += wearable.airDrag * statsModifier;
                totalMass_kg += wearable.mass_kg * statsModifier;
            }

            totalMass_kg += userMass_kg;
        }

        public void Save() {
            Json.Stringify(outfit);

            List<string> data =
            [
                name,
                userMass_kg.ToString(),
                level.ToString(),
                points.ToString(),
                money.ToString(),
                Json.Stringify(outfit),
            ];
            var text = Json.Stringify(data.ToArray(), "\t");

            var file = FileAccess.Open(Path, FileAccess.ModeFlags.Write);
            file.StoreLine(text);
            file.Flush();
            // Calling dispose is needed because otherwise ***.json.temp02*** instead of normal json files are created
            file.Dispose();
        }

        public static UserConfig Load() {
            var userConfig = new UserConfig();
            if (!FileAccess.FileExists(Path)) {
                userConfig.Save();
                return userConfig;
            }
            var content = FileAccess.Open(Path, FileAccess.ModeFlags.Read).GetAsText();
            var data = (Godot.Collections.Array)Json.ParseString(content);
            if (data == null) {
                userConfig.Save();
                return userConfig;
            }
            userConfig = new UserConfig() {
                name = ((string)data[0]),
                userMass_kg = ((float)data[1]),

                level = ((uint)data[2]),
                points = ((float)data[3]),
                money = ((uint)data[4]),

                outfit = (Godot.Collections.Dictionary<Wearable.Type, Wearable>)
                    Json.ParseString((string)data[5]),
            };

            return userConfig;
        }

        public class HashMap { }
    }
}
