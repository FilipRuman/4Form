namespace ForForm.Menu.Game
{
    using System.Collections.Generic;
    using Bike;
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
        public BikeModel currentBikeModel;
        Map.Map map;

        public void SetupBikeModels() {
            Miscs.ClearChildren(bikeModelLayout);
            bikeModelSelections.Clear();
            foreach (var bikeModel in map.bikeModels) {
                var script = bikeModelSelectionPrefab.Instantiate() as SimpleSelectionUI;
                bikeModelLayout.AddChild(script);
                bikeModelSelections.Add(bikeModel, script);

                script.Setup(
                    bikeModel.name,
                    bikeModel.icon,
                    () =>
                    {
                        gameMenu.OnMenuComplete(2);
                        if (bikeModel == currentBikeModel)
                            return;
                        OnBikeModelSelected(bikeModel);
                    }
                );
                ThemeVariants.SetForButton(bikeModel == currentBikeModel, script);
            }
        }

        public void OnBikeModelSelected(BikeModel bikeModel) {
            ThemeVariants.SetForButton(true, bikeModelSelections[bikeModel]);
            if (currentBikeModel != null && bikeModelSelections.ContainsKey(currentBikeModel))
                ThemeVariants.SetForButton(false, bikeModelSelections[currentBikeModel]);
            currentBikeModel = bikeModel;
            SetupBikeStatsUI();
        }

        public void SetupBikeStatsUI() {
            if (currentBikeModel == null)
                return;
            var editable = map.canEditBikeModels;
            bikeMass.Setup(
                currentBikeModel.mass_kg,
                (f) =>
                {
                    currentBikeModel.mass_kg = f;
                },
                editable
            );
            wheelFrictionCoefficient.Setup(
                currentBikeModel.wheelFrictionCoefficient,
                (f) =>
                {
                    currentBikeModel.wheelFrictionCoefficient = f;
                },
                editable
            );
            bikeWheelRadius.Setup(
                currentBikeModel.wheelRadius_m,
                (f) =>
                {
                    currentBikeModel.wheelRadius_m = f;
                },
                editable
            );
            bikeFrontalArea.Setup(
                currentBikeModel.frontalArea_m,
                (f) =>
                {
                    currentBikeModel.frontalArea_m = f;
                },
                editable
            );
        }

        public void OnNewMap(Map.Map _map) {
            map = _map;
            SetupBikeModels();
            SetupBikeStatsUI();
        }

        public override void _Ready() {
            gameMenu.bikeConfigurationMenu = this;
            base._Ready();
        }
    }
}
