namespace ForForm.GameConfig
{
    using System;
    using ForForm.Map.Route;
    using Godot;
    using Map;

    public static class GameSettings {
        public static GameMode CurrentGameMode { get; private set; }
        public static bool gameStarted;
        public static Action onCurrentGameModeChanged;
        public static Action onCurrentRouteChanged;

        public static Route currentRoute { get; private set; }
        public static Map currentMap;

        public static void SetCurrentRoute(Route route) {
            currentRoute = route;

            if (onCurrentRouteChanged != null)
                onCurrentRouteChanged();
        }

        public static void SetCurrentGameMode(GameMode gameMode) {
            CurrentGameMode = gameMode;
            if (onCurrentGameModeChanged != null) // this can happen only in editor
                onCurrentGameModeChanged();

            Bike.BikeStats.dragCoefficient = gameMode.dragCoefficient;
            Bike.BikeStats.frontalArea = gameMode.userDrag;
        }
    }
}
