using System.Text;
using TMPro;
using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class TestVehicleDriver : MonoBehaviour
    {
        [SerializeField] private Vehicle vehicle;
        [SerializeField] private TMP_Text debugText;

        private readonly StringBuilder debugStringBuilder = new();

        private void Update()
        {
            var vehicleInputs = vehicle.Inputs;
            vehicleInputs.Steer = Input.GetAxis("Horizontal");

            var vertical = Input.GetAxis("Vertical");
            vehicleInputs.Throttle = Mathf.Max(vertical, 0);
            vehicleInputs.Brake = Mathf.Max(vertical * -1, 0);

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                vehicle.VehicleTransmission.ShiftUp(vehicle);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                vehicle.VehicleTransmission.ShiftDown();
            }

            debugStringBuilder.AppendLine($"Current Gear: {vehicle.VehicleTransmission.CurrentGear}");
            debugStringBuilder.AppendLine($"Current Gear Ratio: {vehicle.VehicleTransmission.CurrentGearRatio:f2}");
            debugStringBuilder.AppendLine($"Total Drive Ratio: {vehicle.VehicleTransmission.TotalDriveRatio:f2}");
            debugStringBuilder.AppendLine($"Wheel RPM: {vehicle.Wheels.MaxRpm:f0}");
            debugStringBuilder.AppendLine($"Current Motor Torque: {vehicle.CurrentMotorTorque:f0}nm");
            debugStringBuilder.AppendLine($"Current Wheel Torque: {vehicle.Wheels.WheelTorque:f0}nm");
            debugStringBuilder.AppendLine($"Current RPM: {vehicle.VehicleTransmission.CurrentRpm:f0}");
            debugStringBuilder.AppendLine($"Speed: {vehicle.Speed:f0}");
            debugStringBuilder.AppendLine($"TCR cut: {vehicle.TractionControlCut*100:f0}%");
            debugStringBuilder.AppendLine($"ABS cut: {vehicle.Wheels.AbsCut*100:f0}%");
            debugStringBuilder.AppendLine($"Clutch position: {vehicle.ClutchPosition * 100:f0}%");

            debugText.text = debugStringBuilder.ToString();
            debugStringBuilder.Clear();
        }
    }
}