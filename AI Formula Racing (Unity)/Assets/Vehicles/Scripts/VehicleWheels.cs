using System.Collections.Generic;
using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class VehicleWheels : MonoBehaviour
    {
        [SerializeField] private VehicleWheel frontLeft;
        [SerializeField] private VehicleWheel frontRight;
        [SerializeField] private VehicleWheel rearLeft;
        [SerializeField] private VehicleWheel rearRight;

        public VehicleWheel FrontLeft => frontLeft;
        public VehicleWheel FrontRight => frontRight;
        public VehicleWheel RearLeft => rearLeft;
        public VehicleWheel RearRight => rearRight;
        public float MaxRpm { get; private set; }
        public float AbsCut { get; private set; }
        public IReadOnlyList<VehicleWheel> AllWheels { get; private set; }
        public IReadOnlyList<VehicleWheel> FrontWheels { get; private set; }
        public IReadOnlyList<VehicleWheel> RearWheels { get; private set; }

        private void Awake()
        {
            AllWheels = new List<VehicleWheel> { frontLeft, frontRight, rearLeft, rearRight };
            FrontWheels = new List<VehicleWheel> { frontLeft, frontRight };
            RearWheels = new List<VehicleWheel> { rearLeft, rearRight };
        }

        public void UpdateWheels(Vehicle vehicle)
        {
            var inputs = vehicle.Inputs;
            var vehicleBehaviour = vehicle.VehicleBehaviourDescription;
            CalculateAbsCut(vehicle);

            var frontTorque = vehicle.CurrentTorque * vehicleBehaviour.TorqueBias;
            var absBrakeTorqueMultiplier = (1 - AbsCut);
            var frontBrakeTorque = inputs.Brake * vehicleBehaviour.MaxBrakePower * vehicleBehaviour.BrakeBias * absBrakeTorqueMultiplier;
            var steerAngle = inputs.Steer * vehicleBehaviour.MaxSteerAngle;
            foreach (var wheel in FrontWheels)
            {
                wheel.Torque = frontTorque;
                wheel.BrakeTorque = frontBrakeTorque;
                wheel.SteerAngle = steerAngle;
            }

            var rearTorque = vehicle.CurrentTorque * (1 - vehicleBehaviour.TorqueBias);
            var rearBrakeTorque = inputs.Brake * vehicleBehaviour.MaxBrakePower * (1 - vehicleBehaviour.BrakeBias) * absBrakeTorqueMultiplier;
            foreach (var wheel in RearWheels)
            {
                wheel.Torque = rearTorque;
                wheel.BrakeTorque = rearBrakeTorque;
                wheel.SteerAngle = 0;
            }

            MaxRpm = Mathf.Max(rearLeft.Rpm, rearRight.Rpm);
        }

        private void CalculateAbsCut(Vehicle vehicle)
        {
            var desc = vehicle.VehicleBehaviourDescription;

            if (!desc.IsABSEnabled || vehicle.Inputs.Brake == 0f)
            {
                AbsCut = 0;
                return;
            }
            var minimumRpm = Mathf.Min(rearLeft.Rpm, rearRight.Rpm, frontLeft.Rpm, frontRight.Rpm);
            var targetCut = minimumRpm == 0 ? desc.ABSCut : 0;
            AbsCut = Mathf.Lerp(AbsCut, targetCut, Time.deltaTime * desc.ABSSpeed);
            if (Mathf.Abs(AbsCut - targetCut) < 0.001f)
            {
                AbsCut = targetCut;
            }
        }
    }
}
