using System;
using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class AutomaticTransmission : MonoBehaviour
    {
        [SerializeField] private Vehicle vehicle;
        [SerializeField][Range(0, 1)] private float upshiftPosition = 0.9f;
        [SerializeField][Range(0, 1)] private float downshiftPosition = 0.6f;

        private float[] upshiftSpeeds;
        private float[] downshiftSpeeds;

        private void Start()
        {
            var upshiftRpm = vehicle.VehicleBehaviourDescription.MaxRPM * upshiftPosition;
            var downshiftRpm = vehicle.VehicleBehaviourDescription.MaxRPM * downshiftPosition;
            var wheelRadius = vehicle.Wheels.RearLeft.Radius * 100;
            var wheelCircumference = 2 * Mathf.PI * wheelRadius;
            var gearRatios = vehicle.VehicleBehaviourDescription.GearRatios;
            var differentialGearRatio = vehicle.VehicleBehaviourDescription.DifferentialGearRatio;
            upshiftSpeeds = new float[vehicle.VehicleBehaviourDescription.GearCount];
            downshiftSpeeds = new float[vehicle.VehicleBehaviourDescription.GearCount];
            for (int i = 0; i < upshiftSpeeds.Length; i++)
            {
                var finalDriveRatio = gearRatios[i] * differentialGearRatio;
                upshiftSpeeds[i] = (upshiftRpm / finalDriveRatio) * wheelCircumference * 60 / 100_000;
                downshiftSpeeds[i] = (downshiftRpm / finalDriveRatio) * wheelCircumference * 60 / 100_000;
            }
        }

        private void Update()
        {
            var currentSpeed = vehicle.Speed;
            var gears = vehicle.VehicleBehaviourDescription.GearCount;
            var currentGear = vehicle.VehicleTransmission.CurrentGear;
            var currentRpm = vehicle.VehicleTransmission.CurrentRpm;
            var gearRatios = vehicle.VehicleBehaviourDescription.GearRatios;
            if (currentSpeed > upshiftSpeeds[currentGear] && currentGear != gears - 1)
            {
                vehicle.VehicleTransmission.ShiftUp(vehicle);
            }
            else if (currentSpeed < downshiftSpeeds[currentGear] && currentGear != 0)
            {
                vehicle.VehicleTransmission.ShiftDown();
            }
        }
    }
}
