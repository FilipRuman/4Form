namespace ForForm.Bike
{
    using Godot;

    [GlobalClass, Tool]
    public partial class BikeModel : Resource {
        [Export]
        public string name;

        [Export]
        public float mass, //kg
            wheelFrictionCoefficient,
            wheelRadius, //m
            frontalArea;

        [Export]
        public Texture2D icon;

        //Export

        static string BasePath(string mapName, string name) =>
            $"user://Maps/{mapName}/Bikes/{name}/";

        public void Save(string mapName) {
            string _basePath = BasePath(mapName, name);
            DirAccess.Open($"user://Maps/{mapName}/Bikes/").MakeDir(name);

            icon.GetImage().SavePng(_basePath + "icon.png");
            var data = new Godot.Collections.Dictionary {
                { "name", name },
                { "mass", mass },
                { "wheelFrictionCoefficient", wheelFrictionCoefficient },
                { "wheelRadius", wheelRadius },
                { "frontalArea", frontalArea },
            };
            var text = Json.Stringify(data, "\t");

            var file = FileAccess.Open(_basePath + "data.json", FileAccess.ModeFlags.Write);
            file.StoreLine(text);
            file.Flush();

            file.Dispose();
        }

        public static BikeModel Load(string name, string mapName) {
            string _basePath = BasePath(mapName, name);
            string jsonString = FileAccess
                .Open(_basePath + "data.json", FileAccess.ModeFlags.Read)
                .GetAsText();
            var data = ((Godot.Collections.Dictionary)Json.ParseString(jsonString));

            return new BikeModel {
                name = ((string)data["name"]),
                mass = ((float)data["mass"]),
                wheelFrictionCoefficient = ((float)data["wheelFrictionCoefficient"]),
                wheelRadius = ((float)data["wheelRadius"]),
                frontalArea = ((float)data["frontalArea"]),
                icon = ImageTexture.CreateFromImage(Image.LoadFromFile(_basePath + "icon.png")),
            };
        }
    }
}
