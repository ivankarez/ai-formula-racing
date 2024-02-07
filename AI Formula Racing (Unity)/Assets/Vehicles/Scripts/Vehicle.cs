using Ivankarez.AIFR.Common.Utils;
using System.Linq;
using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class Vehicle : MonoBehaviour
    {
        public float CurrentMotorTorque { get; private set; }
        public float TractionControlCut { get; private set; }
        public VehicleInputs Inputs => inputs;
        public VehicleBehaviourDescription VehicleBehaviourDescription => vehicleBehaviourDescription;
        public VehicleWheels Wheels => wheels;
        public VehicleTransmission VehicleTransmission => vehicleTransmission;
        public float Speed => vehicleRigidbody.velocity.magnitude * 3.6f;
        public float ClutchPosition { get; private set; }
        public float TotalDownforce { get; private set; }
        public float DownforceWeightPercentage { get; private set; }

        [Header("Dependencies")]
        [SerializeField] private VehicleWheels wheels;
        [SerializeField] private VehicleInputs inputs;
        [SerializeField] private VehicleTransmission vehicleTransmission;
        [SerializeField] private Rigidbody vehicleRigidbody;

        [Header("Settings")]
        [SerializeField] private VehicleBehaviourDescription vehicleBehaviourDescription;
        [SerializeField] private Transform centerOfMass;


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
            vehicleRigidbody.centerOfMass = centerOfMass.localPosition;
        }

        private void Update()
        {
            VehicleTransmission.UpdateTransmission(this);

            CalculateDownforce();
            CalculateTractionControlCut();
            CalculateAutoClutch();
            CalculateCurrentTorque();

            wheels.UpdateWheels(this);
        }

        private void CalculateCurrentTorque()
        {
            if (vehicleTransmission.CurrentRpm > vehicleBehaviourDescription.MaxRPM)
            {
                CurrentMotorTorque = 0;
            }
            else
            {
                var rpmRatio = VehicleTransmission.CurrentRpm / vehicleBehaviourDescription.MaxRPM;
                var torqueCurveMultiplier = vehicleBehaviourDescription.TorqueCurve.Evaluate(rpmRatio);
                var clutchMutliplier = (1 - ClutchPosition);
                if (inputs.Throttle == 0)
                {
                    var speedMultiplier = Mathf.Clamp01(Speed / 100f);
                    CurrentMotorTorque = torqueCurveMultiplier * -VehicleBehaviourDescription.MaxEngineBrakePower * clutchMutliplier * speedMultiplier;
                }
                else
                {
                    CurrentMotorTorque = inputs.Throttle * torqueCurveMultiplier * vehicleBehaviourDescription.MaxTorque * (1 - TractionControlCut) * clutchMutliplier;
                }
            }
        }

        private void CalculateTractionControlCut()
        {
            if (!VehicleBehaviourDescription.IsTractionControlEnabled || inputs.Throttle == 0)
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

        private void CalculateAutoClutch()
        {
            var targetClutch = vehicleTransmission.CurrentRpm < VehicleBehaviourDescription.IdleRpm && inputs.Throttle == 0 ? 1 : 0;
            ClutchPosition = Mathf.Lerp(ClutchPosition, targetClutch, Time.deltaTime * VehicleBehaviourDescription.ClutchSpeed);
        }

        private void CalculateDownforce()
        {
            TotalDownforce = Speed * vehicleBehaviourDescription.Downforce / 100f;
            vehicleRigidbody.AddForce(-vehicleRigidbody.transform.up * TotalDownforce);
            DownforceWeightPercentage = TotalDownforce / vehicleBehaviourDescription.Mass;
        }
    }
}
