namespace ForForm.Bike
{
    using System;
    using Godot;

    public partial class BikeInput : Node {
        public uint currentWatts;
        public uint currentCadence_RPM;
        public uint wheelRotation_degS; //Deg/s
        public uint heartRate;
    }
}
