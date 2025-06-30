namespace ForForm.Map
{
    using Godot;

    [Tool, Icon("res://Script icons/map_3D_node_color.png")]
    public partial class Map : Node3D {
        [Export]
        public string name;

        [Export(PropertyHint.MultilineText)]
        public string description;

        [Export]
        public Texture2D icon;

        [Export]
        public Route.Route[] routes;

        // Exporting
        static string BasePath(string name) => $"user://Maps/{name}/Map/";

        [Export]
        public float speedScale,
            dragCoefficient;

        //dragCoefficient, canEditBikeModels, bikeModels
        [Export]
        public bool canEditBikeModels;

        [Export]
        public Bike.BikeModel[] bikeModels;

        public void Save() {
            DirAccess.Open($"user://Maps/{name}").MakeDir("Map");
            SaveBikes(name);
            string _basePath = BasePath(name);

            if (icon != null && icon.GetImage() != null)
                icon.GetImage().SavePng(_basePath + "icon.png");

            var data = new Godot.Collections.Dictionary {
                { "name", name },
                { "speedScale", speedScale },
                { "description", description },
                { "dragCoefficient", dragCoefficient },
                { "canEditBikeModels", canEditBikeModels },
            };
            var text = Json.Stringify(data, "\t");

            var file = FileAccess.Open(_basePath + "data.json", FileAccess.ModeFlags.Write);

            file.StoreLine(text);
            file.Flush();

            // Calling dispose is needed because otherwise ***.json.temp02*** instead of normal json files are created
            file.Dispose();
        }

        public static Map Load(string name) {
            var _basePath = BasePath(name);

            string jsonString = FileAccess
                .Open(_basePath + "data.json", FileAccess.ModeFlags.Read)
                .GetAsText();
            var data = ((Godot.Collections.Dictionary)Json.ParseString(jsonString));
            return new Map {
                name = ((string)data["name"]),
                description = ((string)data["description"]),
                icon = ImageTexture.CreateFromImage(Image.LoadFromFile(_basePath + "icon.png")),
                speedScale = ((float)data["speedScale"]),
                dragCoefficient = ((float)data["dragCoefficient"]),
                canEditBikeModels = ((bool)data["canEditBikeModels"]),
                bikeModels = LoadBikes(name),
            };
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
