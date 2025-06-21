namespace ForForm.Map
{
    using System.IO;
    using Godot;

    public partial class InitialMapsImporter : Node {
        [Export]
        Menu.Game.MapSelectionMenu mapSelectionMenu;
        private static readonly string[] possiblePaths =
        {
            "./InitialMaps/",
            "./../InitialMaps/",
        };

        // i don't use zip files because you can't see their contents inside git
        public override void _Ready() {
            if (DirAccess.DirExistsAbsolute("user://Maps"))
                return;
            var working_path = "none of the paths works!";
            foreach (string path in possiblePaths) {
                if (Directory.Exists(path)) {
                    working_path = path;
                    break;
                }
            }
            CopyFolder(working_path, "user://Maps");
            mapSelectionMenu.DisplayMaps();
            base._Ready();
        }

        public void CopyFolder(string sourcePath, string destinationPath) {
            var sourceDir = DirAccess.Open(sourcePath);
            DirAccess.MakeDirAbsolute(destinationPath);
            foreach (var dir in sourceDir.GetDirectories()) {
                CopyFolder($"{sourcePath}/{dir}", $"{destinationPath}/{dir}");
            }
            foreach (var file in sourceDir.GetFiles()) {
                DirAccess.CopyAbsolute($"{sourcePath}/{file}", $"{destinationPath}/{file}");
            }
        }
    }
}
