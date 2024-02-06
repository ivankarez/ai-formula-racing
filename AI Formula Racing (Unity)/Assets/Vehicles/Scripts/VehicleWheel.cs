using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class VehicleWheel : MonoBehaviour
    {
        public float SteerAngle { get; set; }
        public float Torque { get; set; }
        public float BrakeTorque { get; set; }
        public float Rpm { get; private set; }
        public float ForwardSlip { get; private set; }
        public float SidewaysSlip { get; private set; }
        public bool IsGrounded { get; private set; }

        [SerializeField] private WheelCollider wheelCollider;
        [SerializeField] private Transform wheelTransform;

        private Vector3 wheelPosition;
        private Quaternion wheelRotation;

        private void Awake()
        {
            wheelCollider.ConfigureVehicleSubsteps(5.0f, 30, 30);
        }

        private void Update()
        {
            wheelCollider.steerAngle = SteerAngle;
            wheelCollider.motorTorque = Torque;
            wheelCollider.brakeTorque = BrakeTorque;

            wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);
            wheelTransform.position = wheelPosition;
            wheelTransform.rotation = wheelRotation;

            if (wheelCollider.GetGroundHit(out var hit))
            {
                ForwardSlip = hit.forwardSlip;
                SidewaysSlip = hit.sidewaysSlip;
                IsGrounded = true;
            }
            else
            {
                ForwardSlip = 0;
                SidewaysSlip = 0;
                IsGrounded = false;
            }

            Rpm = wheelCollider.rpm;
        }
    }
}
