namespace ForForm.Map
{
    using Godot;

    [Tool,Icon("res://Script icons/map_3D_node_color.png")]
    public partial class Map : Node3D {
        [Export]
        public string name;

        [Export(PropertyHint.MultilineText)]
        public string description;

        [Export]
        public Texture2D icon;

        [Export]
        public GameConfig.GameMode gameMode;

        [Export]
        public Route.Route[] routes;

        [Export]
        public Node3D terrain3D;

        // Exporting
        static string BasePath(string name) => $"user://Maps/{name}/Map/";

        public void Save() {
            DirAccess.Open($"user://Maps/{name}").MakeDir("Map");
            string _basePath = BasePath(name);

            if (icon != null && icon.GetImage() != null)
                icon.GetImage().SavePng(_basePath + "icon.png");

            var data = new Godot.Collections.Dictionary {
                { "name", name },
                { "description", description },
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
            };
        }
    }
}
