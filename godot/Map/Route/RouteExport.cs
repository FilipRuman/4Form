namespace ForForm.Map.Route
{
    using System;
    using Godot;

    [Tool,Icon("res://Script icons/publish.png")]
    public partial class RouteExport : Node3D {
        string RouteSpecificBasePath(string routeName) =>
            $"user://Maps/{map.name}/Routes/{routeName}/";

        string AllRoutesPath => $"user://Maps/{map.name}/Routes/";

        [Export]
        Map map;

        public void ExportRoutes() {
            DirAccess.Open($"user://Maps/{map.name}").MakeDir("Routes");

            foreach (var route in map.routes) {
                DirAccess.Open($"user://Maps/{map.name}/Routes").MakeDir(route.name);
                string _basePath = RouteSpecificBasePath(route.name);

                if (route.icon != null)
                    route.icon.GetImage().SavePng(_basePath + "icon.png");

                var data = new Godot.Collections.Dictionary {
                    { "name", route.name },
                    { "Position", route.GlobalPosition },
                    { "startingPoint", route.startingPoint },
                    { "difficulty", route.difficulty },
                    { "estimatedTime", route.estimatedTime },
                    { "totalDistance", route.totalDistance },
                    { "ascent", route.ascent },
                    // curve 3D data
                    { "Closed", route.Curve.Closed },
                    { "BakeInterval", route.Curve.BakeInterval },
                    { "Points", route.Curve.GetBakedPoints() },
                };
                var text = Json.Stringify(data, "\t");

                var file = FileAccess.Open(_basePath + "data.json", FileAccess.ModeFlags.Write);
                file.StoreLine(text);
                file.Flush();
                
            // Calling dispose is needed because otherwise ***.json.temp02*** instead of normal json files are created 
                file.Dispose();
            }
        }

        [Export]
        PackedScene routePrefab;

        public void ImportRoutes() {
            Menu.UIMiscs.ClearChildren(this);
            var routeDirs = DirAccess.GetDirectoriesAt(AllRoutesPath);
            var routeArray = new Route[routeDirs.Length];
            var i = 0;
            foreach (var routeName in routeDirs) {
                var route = routePrefab.Instantiate() as Route;
                AddChild(route);

                string _basePath = RouteSpecificBasePath(routeName);
                route.icon = ImageTexture.CreateFromImage(
                    Image.LoadFromFile(_basePath + "icon.png")
                );
                routeArray[i] = route;

                var file = FileAccess.Open(_basePath + "data.json", FileAccess.ModeFlags.Read);

                var data = ((Godot.Collections.Dictionary)Json.ParseString(file.GetAsText()));

                route.GlobalPosition = ((Vector3)data["Position"]);
                route.name = ((string)data["name"]);
                route.startingPoint = ((float)data["startingPoint"]);
                route.difficulty = ((string)data["difficulty"]);
                route.estimatedTime = ((uint)data["estimatedTime"]);
                route.totalDistance = ((float)data["totalDistance"]);
                route.ascent = ((float)data["ascent"]);
                // curve 3D data
                route.Curve.Closed = ((bool)data["Closed"]);
                route.Curve.BakeInterval = ((float)data["BakeInterval"]);
                route.Curve.ClearPoints();
                foreach (var point in ((Godot.Collections.Array)data["Points"])) {
                    route.Curve.AddPoint(((Vector3)point));
                }

                i++;
            }
            map.routes = routeArray;
        }
    }
}
