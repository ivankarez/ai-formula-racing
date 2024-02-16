using Ivankarez.AIFR.Common.Utils;
using Ivankarez.AIFR.TrainingAlgorithm;
using Ivankarez.AIFR.Vehicles;
using Ivankarez.NeuralNetworks;
using UnityEngine;

namespace Ivankarez.AIFR
{
    public class TrainingDriver : MonoBehaviour
    {
        public Vehicle Vehicle => vehicle;
        public float DistanceTravelled { get; private set; }
        public float TimeAlive { get; private set; }
        public AiVision VehicleCamera => vehicleCamera;

        [SerializeField] private NeuralNetworkProvider networkProvider;
        [SerializeField] private Vehicle vehicle;
        [SerializeField] private AiVision vehicleCamera;

        private Individual individual;
        private bool isInitialized = false;
        private Vector3 lastPosition;
        private LayeredNetworkModel embeddingModel;
        private LayeredNetworkModel drivingModel;

        private void Awake()
        {
            Check.ArgumentNotNull(networkProvider);
            Check.ArgumentNotNull(vehicle);
        }

        public void Initialize(Individual individual)
        {
            Check.State(!isInitialized, "Cannot initialize a training driver mutliple times");

            this.individual = Check.ArgumentNotNull(individual);
            vehicle.gameObject.SetActive(true);
            isInitialized = true;
            lastPosition = vehicle.transform.position;
            TimeAlive = 0f;
            embeddingModel = networkProvider.CreateImageEmbeddingModel();
            embeddingModel.SetParametersFlat(individual.EmbeddingWeights);
            drivingModel = networkProvider.CreateDrivingModel();
            drivingModel.SetParametersFlat(individual.DrivingNetworkWeights);
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            vehicleCamera.UpdateValues();
            var imageVectors = embeddingModel.Feedforward(vehicleCamera.Values);
            var output = drivingModel.Feedforward(imageVectors);
            vehicle.Inputs.Throttle = Mathf.Max(output[0], 0);
            vehicle.Inputs.Brake = Mathf.Max(-output[0], 0);
            vehicle.Inputs.Steer = output[1];

            var distance = Vector3.Distance(lastPosition, vehicle.transform.position);
            DistanceTravelled += distance;
            lastPosition = vehicle.transform.position;
            TimeAlive += Time.deltaTime;

            individual.Fitness = distance;
            individual.TimeAlive = TimeAlive;
        }
    }
}
