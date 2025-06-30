namespace ForForm.Menu
{
    using System.Collections.Generic;
    using Godot;

    public  class UserConfig  {
        [Export]
        public string name = "";

        [Export]
        public float mass_kg;
        public const string Path = "user://UserConfig.json";

        public void Save() {
            List<string> data = [name, mass_kg.ToString()];
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

            userConfig.name = ((string)data[0]);
            userConfig.mass_kg = ((float)data[1]);
            return userConfig;
        }
    }
}

