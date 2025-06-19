namespace ForForm.Menu.Game
{
    using System.Collections.Generic;
    using Bike;
    using ForForm.GameConfig;
    using Godot;

    public partial class BikeConfigurationMenu : Control {
        [Export]
        PropertyEdit bikeMass,
            wheelFrictionCoefficient,
            bikeWheelRadius,
            bikeFrontalArea;

        [Export]
        PackedScene bikeModelSelectionPrefab;

        [Export]
        GameMenu gameMenu;

        [Export]
        Control bikeModelLayout;
        Dictionary<BikeModel, SimpleSelectionUI> bikeModelSelections = new();

        public void SetupBikeModels() {
            UIMiscs.ClearChildren(bikeModelLayout);
            bikeModelSelections.Clear();
            foreach (var bikeModel in GameSettings.CurrentGameMode.bikeModels) {
                var script = bikeModelSelectionPrefab.Instantiate() as SimpleSelectionUI;
                bikeModelLayout.AddChild(script);
                bikeModelSelections.Add(bikeModel, script);

                script.Setup(
                    bikeModel.name,
                    bikeModel.icon,
                    () =>
                    {
                        gameMenu.OnMenuComplete(2);
                        if (bikeModel == BikeStats.bikeModel)
                            return;
                        OnBikeModelSelected(bikeModel);
                    }
                );
                ThemeVariants.SetForButton(bikeModel == BikeStats.bikeModel, script);
            }
        }

        public void OnBikeModelSelected(BikeModel bikeModel) {
            ThemeVariants.SetForButton(true, bikeModelSelections[bikeModel]);
            if (BikeStats.bikeModel != null && bikeModelSelections.ContainsKey(BikeStats.bikeModel))
                ThemeVariants.SetForButton(false, bikeModelSelections[BikeStats.bikeModel]);
            BikeStats.bikeModel = bikeModel;
            SetupBikeStatsUI();
        }

        public void SetupBikeStatsUI() {
            if (BikeStats.bikeModel == null)
                return;
            var editable = GameConfig.GameSettings.CurrentGameMode.canEditBikeModels;
            bikeMass.Setup(
                BikeStats.bikeModel.mass,
                (f) =>
                {
                    BikeStats.bikeModel.mass = f;
                },
                editable
            );
            wheelFrictionCoefficient.Setup(
                BikeStats.bikeModel.wheelFrictionCoefficient,
                (f) =>
                {
                    BikeStats.bikeModel.wheelFrictionCoefficient = f;
                },
                editable
            );
            bikeWheelRadius.Setup(
                BikeStats.bikeModel.wheelRadius,
                (f) =>
                {
                    BikeStats.bikeModel.wheelRadius = f;
                },
                editable
            );
            bikeFrontalArea.Setup(
                BikeStats.bikeModel.frontalArea,
                (f) =>
                {
                    BikeStats.bikeModel.frontalArea = f;
                },
                editable
            );
        }

        public void OnCurrentGameModeChanged() {
            BikeStats.dragCoefficient = GameSettings.CurrentGameMode.dragCoefficient;
            BikeStats.frontalArea = GameSettings.CurrentGameMode.userDrag;
            SetupBikeModels();
            SetupBikeStatsUI();
        }

        public override void _Ready() {
            GameSettings.onCurrentGameModeChanged += OnCurrentGameModeChanged;
            base._Ready();
        }
    }
}
