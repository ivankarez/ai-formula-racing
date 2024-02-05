using UnityEngine;

namespace Ivankarez.AIFR.Vehicles
{
    [CreateAssetMenu(fileName = "VehicleBehaviourDescription", menuName = "Ivankarez/Vehicle Behaviour Description", order = 1)]
    public class VehicleBehaviourDescription : ScriptableObject
    {
        public float Mass => mass;
        public float MaxSpeed => maxSpeed;
        public float MaxTorque => maxTorque;
        public float MaxRPM => maxRPM;
        public AnimationCurve TorqueCurve => torqueCurve;   
        public float MaxEngineBrakePower => maxEngineBrakePower;
        public float TorqueBias => torqueBias;
        public float MaxBrakePower => maxBrakePower;
        public float BrakeBias => brakeBias;
        public float MaxSteerAngle => maxSteerAngle;
        public float[] GearRatios => gearRatios;
        public int GearCount => gearRatios.Length;
        public float DifferentialGearRatio => differentialGearRatio;


        [Header("Physical")]
        [SerializeField][Tooltip("Mass of the vehicle in kilogramms")] private float mass = 1000;
        [SerializeField][Tooltip("Total downforce of the vehicle (in newtons) at 100km/h")] private float maxSpeed = 20;

        [Header("Engine")]
        [SerializeField][Tooltip("Maximum engine torque (in newton meters)")] private float maxTorque = 500;
        [SerializeField][Tooltip("Maximum engine RPM")] private float maxRPM = 6000;
        [SerializeField][Tooltip("Torque curve of the engine. Where the X axis is the RPM % [0-1], and Y is the torque % [0-1]")] private AnimationCurve torqueCurve;
        [SerializeField][Tooltip("Engine brake power at maximum RPM (in newton meters)")] private float maxEngineBrakePower = 10;
        [SerializeField][Tooltip("Distribution of motor torque between the front axle and the rear axle. (0=RWD, 1=FWD)")] private float torqueBias = 0f;

        [Header("Brake")]
        [SerializeField][Tooltip("Maximum brake power (in newton meters)")] private float maxBrakePower = 1000;
        [SerializeField][Tooltip("Distribution of brake torque between the front axle and the rear axle. (0=RWD, 1=FWD)")] private float brakeBias = 0.6f;

        [Header("Steering")]
        [SerializeField][Tooltip("Maximum steering angle (in degrees)")] private float maxSteerAngle = 30;

        [Header("Transmission")]
        [SerializeField][Tooltip("Gear ratios")] private float[] gearRatios = new float[] { 3.5f, 2.5f, 1.8f, 1.3f, 1.0f, 0.8f };
        [SerializeField][Tooltip("Differential gear ratio")] private float differentialGearRatio = 3.5f;

    }
}
