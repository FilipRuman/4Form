namespace ForForm.Menu.BikeConfig
{
    using System.Collections.Generic;
    using Bike;
    using ForForm.GameConfig;
    using Godot;

    public partial class BikeConfigurationMenu : Control {
        [ExportGroup("user settings")]
        [Export]
        PropertyEdit userMass,
            dragCoefficient,
            frontalArea;

        [ExportGroup("bike settings")]
        [Export]
        PropertyEdit bikeMass,
            wheelFrictionCoefficient,
            bikeWheelRadius,
            bikeFrontalArea;

        [Export]
        PackedScene bikeModelSelectionPrefab;

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
                        OnBikeModelSelected(bikeModel);
                    }
                );
                ThemeVariants.SetForButton(bikeModel == BikeStats.bikeModel, script);
            }
        }

        public void OnBikeModelSelected(BikeModel bikeModel) {
            ThemeVariants.SetForButton(true, bikeModelSelections[bikeModel]);
            ThemeVariants.SetForButton(false, bikeModelSelections[BikeStats.bikeModel]);
            BikeStats.bikeModel = bikeModel;
            SetupBikeStatsUI();
        }

        public void SetupBikeStatsUI() {
            bikeMass.Setup(
                BikeStats.bikeModel.mass,
                (f) =>
                {
                    BikeStats.bikeModel.mass = f;
                }
            );
            wheelFrictionCoefficient.Setup(
                BikeStats.bikeModel.wheelFrictionCoefficient,
                (f) =>
                {
                    BikeStats.bikeModel.wheelFrictionCoefficient = f;
                }
            );
            bikeWheelRadius.Setup(
                BikeStats.bikeModel.wheelRadius,
                (f) =>
                {
                    BikeStats.bikeModel.wheelRadius = f;
                }
            );
            bikeFrontalArea.Setup(
                BikeStats.bikeModel.frontalArea,
                (f) =>
                {
                    BikeStats.bikeModel.frontalArea = f;
                }
            );
        }

        public void OnCurrentGameModeChanged() {
            BikeStats.bikeModel = GameSettings.CurrentGameMode.bikeModels[0];
            BikeStats.dragCoefficient = GameSettings.CurrentGameMode.dragCoefficient;
            BikeStats.frontalArea = GameSettings.CurrentGameMode.userDrag;
            SetupBikeModels();
            SetupBikeStatsUI();
            userMass.Setup(
                BikeStats.userMass,
                (f) =>
                {
                    BikeStats.userMass = f;
                }
            );
            dragCoefficient.Setup(
                BikeStats.dragCoefficient,
                (f) =>
                {
                    BikeStats.dragCoefficient = f;
                }
            );
            frontalArea.Setup(
                BikeStats.frontalArea,
                (f) =>
                {
                    BikeStats.frontalArea = f;
                }
            );
        }

        public override void _Ready() {
            GameSettings.onCurrentGameModeChanged += OnCurrentGameModeChanged;
            base._Ready();
        }
    }
}
