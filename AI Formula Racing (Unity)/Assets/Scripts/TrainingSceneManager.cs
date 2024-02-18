using Ivankarez.AIFR.Common.Utils;
using Ivankarez.AIFR.TrainingAlgorithm;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Ivankarez.AIFR
{
    public class TrainingSceneManager : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<TrainingDriver> OnDriverStarted;
        [HideInInspector] public UnityEvent<TrainingDriver> OnDriverFinished;

        [SerializeField] private GeneticAlgorithm geneticAlgorithm;
        [SerializeField] private TrainingDriver driverPrefab;
        [SerializeField] private AIFRSettingsProvider settingsProvider;

        private readonly Queue<Individual> testQueue = new();
        private readonly List<TrainingDriver> runningDrivers = new();
        private bool isTesting = false;
        private RaceTrack raceTrack = null;
        private float lastSpawnTime = 0f;

        private void Awake()
        {
            Check.ArgumentNotNull(geneticAlgorithm, nameof(geneticAlgorithm));
            Check.ArgumentNotNull(driverPrefab, nameof(driverPrefab));
            Check.ArgumentNotNull(settingsProvider, nameof(settingsProvider));            
        }

        private void Start()
        {
            driverPrefab.gameObject.SetActive(false);
        }

        public void OnRaceTrackLoaded(RaceTrack track)
        {
            Check.ArgumentNotNull(track, nameof(track));
            Check.State(raceTrack == null, "Cannot load multiple track in a single training session.");
            raceTrack = track;

            LoadNextBatch();
        }

        public void LoadNextBatch()
        {
            Check.State(!isTesting, "Cannot load next batch while test already running.");
            Check.State(runningDrivers.Count == 0, "Cannot start new testing batch when there is still running drivers on track.");
            Check.State(testQueue.Count == 0, "Cannot start new testing batch when the queue is not empty.");
            var individualsToTest = geneticAlgorithm.GetIndividualsToTest();

            Check.State(individualsToTest.Count() > 0, "Genetic algorithm provided no individuals to test.");
            testQueue.EnqueueAll(individualsToTest);

            isTesting = true;
        }

        private void Update()
        {
            if (!isTesting)
            {
                return;
            }

            var finishedDrivers = runningDrivers.Where(IsDriverFinished).ToList();
            foreach (var driver in finishedDrivers)
            {
                runningDrivers.Remove(driver);
                OnDriverFinished.Invoke(driver);
                Destroy(driver.gameObject);
            }

            var settings = settingsProvider.Settings;
            if (Time.realtimeSinceStartup - lastSpawnTime >= settings.VehicleSpawnDelay || runningDrivers.Count == 0)
            {
                if (testQueue.Count > 0 && runningDrivers.Count < settings.ConcurrentVehicleLimit)
                {
                    SpawnDriver();
                }
            }

            if (testQueue.Count == 0 && runningDrivers.Count == 0)
            {
                isTesting = false;
                geneticAlgorithm.CreateNextGeneration(LoadNextBatch);
            }
        }

        private void SpawnDriver()
        {
            var settings = settingsProvider.Settings;
            Check.State(runningDrivers.Count < settings.ConcurrentVehicleLimit, "Cannot start new driver because the concurrent vehicle limit is reached");

            var individual = testQueue.Dequeue();
            var driver = Instantiate(driverPrefab, raceTrack.StartPoint.position, raceTrack.StartPoint.rotation);
            driver.gameObject.SetActive(true);
            driver.gameObject.name = $"Driver #{individual.Id}";
            driver.Initialize(individual);
            runningDrivers.Add(driver);

            lastSpawnTime = Time.realtimeSinceStartup;
            OnDriverStarted.Invoke(driver);
        }

        private bool IsDriverFinished(TrainingDriver driver)
        {
            if (driver.TimeAlive > 1 && driver.DistanceTravelled < .5f)
            {
                return true;
            }

            return driver.Vehicle.Wheels.AllWheels.Any(w => w.LatestGroundHit.HasValue && !w.LatestGroundHit.Value.collider.gameObject.CompareTag("Track"));
        }
    }
}
