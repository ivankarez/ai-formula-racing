using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class VehicleTransmission : MonoBehaviour
    {
        public int CurrentGear { get; private set; } = 0;
        public float CurrentGearRatio { get; private set; }
        public float TotalDriveRatio { get; private set; }
        public float CurrentRpm { get; private set; }

        public void UpdateTransmission(Vehicle vehicle)
        {
            CurrentGearRatio = vehicle.VehicleBehaviourDescription.GearRatios[CurrentGear];
            TotalDriveRatio = vehicle.VehicleBehaviourDescription.DifferentialGearRatio * CurrentGearRatio;

            var wheelRpm = vehicle.Wheels.MaxRpm;
            var wheelBasedEngineRpm = wheelRpm * TotalDriveRatio;
            var neutralRpm = CalculateNeutralRpm(vehicle);
            CurrentRpm = Mathf.Lerp(wheelBasedEngineRpm, neutralRpm, vehicle.ClutchPosition);
        }

        public void ShiftUp(Vehicle vehicle)
        {
            if (CurrentGear < vehicle.VehicleBehaviourDescription.GearRatios.Length - 1)
            {
                CurrentGear++;
            }
        }

        public void ShiftDown()
        {
            if (CurrentGear > 0)
            {
                CurrentGear--;
            }
        }

        private float CalculateNeutralRpm(Vehicle vehicle)
        {
            if (vehicle.Inputs.Throttle == 0)
            {
                return vehicle.VehicleBehaviourDescription.IdleRpm;
            }

            return vehicle.VehicleBehaviourDescription.MaxRPM * vehicle.Inputs.Throttle;
        }
    }
}
