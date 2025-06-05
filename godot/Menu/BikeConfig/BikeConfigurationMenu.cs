namespace ForForm.Menu.BikeConfig
{
    using System.Collections.Generic;
    using Bike;
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
        BikeModel[] bikeModels;

        [Export]
        PackedScene bikeModelSelectionPrefab;

        [Export]
        bool canEditBikeModelSettings;

        [Export]
        Control BikeModelLayout;
        Dictionary<BikeModel, BikeModelSelectionUI> bikeModelSelections = new();

        public void SetupBikeModels() {
            foreach (var bikeModel in bikeModels) {
                var script = bikeModelSelectionPrefab.Instantiate() as BikeModelSelectionUI;
                BikeModelLayout.AddChild(script);
                bikeModelSelections.Add(bikeModel, script);

                script.Setup(
                    bikeModel,
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
                },
                canEditBikeModelSettings
            );
            wheelFrictionCoefficient.Setup(
                BikeStats.bikeModel.wheelFrictionCoefficient,
                (f) =>
                {
                    BikeStats.bikeModel.wheelFrictionCoefficient = f;
                },
                canEditBikeModelSettings
            );
            bikeWheelRadius.Setup(
                BikeStats.bikeModel.wheelRadius,
                (f) =>
                {
                    BikeStats.bikeModel.wheelRadius = f;
                },
                canEditBikeModelSettings
            );
            bikeFrontalArea.Setup(
                BikeStats.bikeModel.frontalArea,
                (f) =>
                {
                    BikeStats.bikeModel.frontalArea = f;
                },
                canEditBikeModelSettings
            );
        }

        public override void _Ready() {
            if (BikeStats.bikeModel == null)
                BikeStats.bikeModel = bikeModels[0];
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
            base._Ready();
        }
    }
}
