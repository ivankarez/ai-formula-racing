using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    public class VehicleInputs : MonoBehaviour
    {
        public float Throttle { get; set; } = 0f;
        public float Brake { get; set; } = 0f;
        public float Steer { get; set; } = 0f;
    }
}
