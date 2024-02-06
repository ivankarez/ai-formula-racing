using Ivankarez.AIFR.Common.Utils;
using System.Linq;
using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class Vehicle : MonoBehaviour
    {
        public float CurrentTorque { get; private set; }
        public float TractionControlCut { get; private set; }
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

            CalculateTractionControlCut();
            CalculateCurrentTorque();

            wheels.UpdateWheels(this);
        }

        private void CalculateCurrentTorque()
        {
            if (vehicleTransmission.CurrentRpm > vehicleBehaviourDescription.MaxRPM)
            {
                CurrentTorque = 0;
            }
            else
            {
                var rpmRatio = VehicleTransmission.CurrentRpm / vehicleBehaviourDescription.MaxRPM;
                CurrentTorque = inputs.Throttle * vehicleBehaviourDescription.TorqueCurve.Evaluate(rpmRatio) * vehicleBehaviourDescription.MaxTorque * (1 - TractionControlCut);
            }
        }

        private void CalculateTractionControlCut()
        {
            if (!VehicleBehaviourDescription.IsTractionControlEnabled)
            {
                TractionControlCut = 0;
                return;
            }

            var currentSlip = wheels.AllWheels.Max(w => w.ForwardSlip);
            var targetCut = currentSlip > VehicleBehaviourDescription.TractionControlThreshold ? vehicleBehaviourDescription.TractionControlCut : 0;
            var cut = Mathf.Lerp(TractionControlCut, targetCut, Time.deltaTime * VehicleBehaviourDescription.TractionControlSpeed);
            if (Mathf.Abs(targetCut - cut) < 0.001f)
            {
                cut = targetCut;
            }

            TractionControlCut = cut;
        }
    }
}
