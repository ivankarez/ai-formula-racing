using UnityEngine;

namespace Ivankarez.AIFR
{
    [CreateAssetMenu(fileName = "AIFRSettings", menuName = "Ivankarez/AIFR Settings", order = 2)]
    public class AIFRSettings : ScriptableObject
    {
        [Header("Genetic Algorithm Settings")]
        [SerializeField] private int popultaionSize = 100;
        [SerializeField] private int tournamentSize = 5;
        [SerializeField] private int survivorCount = 50;
        [SerializeField] private float tournamentSelectionChance = 0.5f;
        [SerializeField] private float mutationRate = 0.01f;
        [SerializeField] private float mutationAmount = 0.1f;

        [Header("Training Settings")]
        [SerializeField] private float vehicleSpawnDelay = 1f;
        [SerializeField] private int concurrentVehicleLimit = 5;

        [Header("AI Model Settings")]
        [SerializeField] private int imageEmbeddingVectorSize = 5;
        [SerializeField] private int cameraResolution = 64;


        public int PopultaionSize => popultaionSize;
        public int TournamentSize => tournamentSize;
        public int SurvivorCount => survivorCount;
        public float TournamentSelectionChance => tournamentSelectionChance;
        public float MutationRate => mutationRate;
        public float MutationAmount => mutationAmount;

        public float VehicleSpawnDelay => vehicleSpawnDelay;
        public int ConcurrentVehicleLimit => concurrentVehicleLimit;

        public int ImageEmbeddingVectorSize => imageEmbeddingVectorSize;
        public int CameraResolution => cameraResolution;
    }
}
