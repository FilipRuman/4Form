namespace ForForm.GameConfig
{
    using System;
    using Godot;

    public static class GameSettings {
        public static GameMode CurrentGameMode { get; private set; }
        public static bool gameStarted;
        public static Action onCurrentGameModeChanged;

        public static Map.Route.Route currentRoute;
        public static Map.Map currentMap;

        public static void SetCurrentGameMode(GameMode gameMode) {
            CurrentGameMode = gameMode;
            if(onCurrentGameModeChanged != null)// this can happen only in editor
            onCurrentGameModeChanged();

            Bike.BikeStats.dragCoefficient = gameMode.dragCoefficient;
            Bike.BikeStats.frontalArea = gameMode.userDrag;
        }
    }
}
