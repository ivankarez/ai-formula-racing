using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class VehicleWheel : MonoBehaviour
    {
        public float SteerAngle { get; set; }
        public float Torque { get; set; }
        public float BrakeTorque { get; set; }
        public float Rpm { get; set; }

        [SerializeField] private WheelCollider wheelCollider;
        [SerializeField] private Transform wheelTransform;

        private Vector3 wheelPosition;
        private Quaternion wheelRotation;

        private void Awake()
        {
            wheelCollider.ConfigureVehicleSubsteps(5.0f, 30, 10);
        }

        private void Update()
        {
            wheelCollider.steerAngle = SteerAngle;
            wheelCollider.motorTorque = Torque;
            wheelCollider.brakeTorque = BrakeTorque;

            wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);
            wheelTransform.position = wheelPosition;
            wheelTransform.rotation = wheelRotation;

            Rpm = wheelCollider.rpm;
        }
    }
}
