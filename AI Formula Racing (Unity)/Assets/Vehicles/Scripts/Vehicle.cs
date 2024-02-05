using Ivankarez.AIFR.Common.Utils;
using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class Vehicle : MonoBehaviour
    {
        public float CurrentTorque { get; private set; }
        public VehicleInputs Inputs => inputs;
        public VehicleBehaviourDescription VehicleBehaviourDescription => vehicleBehaviourDescription;
        public VehicleWheels Wheels => wheels;
        public VehicleTransmission VehicleTransmission => vehicleTransmission;
        public float Speed => vehicleRigidbody.velocity.magnitude * 3.6f;

        [Header("Dependencies")]
        [SerializeField] private VehicleWheels wheels;
        [SerializeField] private VehicleInputs inputs;
        [SerializeField] private VehicleTransmission vehicleTransmission;
        [SerializeField] private Rigidbody vehicleRigidbody;

        [Header("Behaviour")]
        [SerializeField] private VehicleBehaviourDescription vehicleBehaviourDescription;


        private void Awake()
        {
            Check.DependencyNotNull(wheels);
            Check.DependencyNotNull(inputs);
            Check.DependencyNotNull(vehicleRigidbody);
            Check.DependencyNotNull(vehicleBehaviourDescription);
            Check.DependencyNotNull(vehicleTransmission);
        }

        private void Start()
        {
            vehicleRigidbody.mass = vehicleBehaviourDescription.Mass;
        }

        private void Update()
        {
            VehicleTransmission.UpdateTransmission(this);

            if (vehicleTransmission.CurrentRpm > vehicleBehaviourDescription.MaxRPM)
            {
                CurrentTorque = 0;
            }
            else
            {
                var rpmRatio = VehicleTransmission.CurrentRpm / vehicleBehaviourDescription.MaxRPM;
                CurrentTorque = inputs.Throttle * vehicleBehaviourDescription.TorqueCurve.Evaluate(rpmRatio) * vehicleBehaviourDescription.MaxTorque;
            }

            wheels.UpdateWheels(this);
        }
    }
}
