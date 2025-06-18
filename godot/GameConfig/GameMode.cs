namespace ForForm.GameConfig
{
    using Godot;

    [GlobalClass,Tool]
    public partial class GameMode : Resource {
        [ExportGroup("bike models")]
        [Export]
        public bool canEditBikeModels;

        [Export]
        public Bike.BikeModel[] bikeModels;

        [ExportGroup("drag settings")]
        [Export]
        public float dragCoefficient = 0;

        [Export]
        public float userDrag = 0;

        // Export
        static string BasePath(string mapName) => $"user://Maps/{mapName}/GameMode/";

        public void Save(string mapName) {
            SaveBikes(mapName);
            DirAccess.Open($"user://Maps/{mapName}").MakeDir("GameMode");

            string _basePath = BasePath(mapName);

            var data = new Godot.Collections.Dictionary {
                { "canEditBikeModels", canEditBikeModels },
                { "dragCoefficient", dragCoefficient },
                { "userDrag", userDrag },
            };
            var text = Json.Stringify(data, "\t");

            var file = FileAccess.Open(_basePath + "data.json", FileAccess.ModeFlags.Write);

            file.StoreLine(text);
            file.Flush();


            // Calling dispose is needed because otherwise ***.json.temp02*** instead of normal json files are created 
            file.Dispose();
        }

        private void SaveBikes(string mapName) {
            var dir = DirAccess.Open($"user://Maps/{mapName}");
            dir.Remove("Bikes");
            // clear old bike models so they don't interfere
            dir.MakeDir("Bikes");
            foreach (var bike in bikeModels) {
                bike.Save(mapName);
            }
        }

        public static GameMode Load(string mapName) {
            string _basePath = BasePath(mapName);
            string jsonString = FileAccess
                .Open(_basePath + "data.json", FileAccess.ModeFlags.Read)
                .GetAsText();
            var data = ((Godot.Collections.Dictionary)Json.ParseString(jsonString));

            return new GameMode {
                canEditBikeModels = ((bool)data["canEditBikeModels"]),
                dragCoefficient = ((float)data["dragCoefficient"]),
                userDrag = ((float)data["userDrag"]),

                bikeModels = LoadBikes(mapName),
            };
        }

        private static Bike.BikeModel[] LoadBikes(string mapName) {
            var bikeNames = DirAccess.GetDirectoriesAt($"user://Maps/{mapName}/Bikes/");
            var output = new Bike.BikeModel[bikeNames.Length];
            var i = 0;
            foreach (var _bikeName in bikeNames) {
                output[i] = Bike.BikeModel.Load(_bikeName, mapName);
                i++;
            }
            return output;
        }
    }
}
