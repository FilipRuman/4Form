namespace ForForm.Bike
{
    using System;
    using Godot;

    public partial class BikeInput : Node {
        public uint currentPower; // Watts
        public uint currentCadence; //RPM
        public uint wheelRotation; //Deg/s
    }
}
