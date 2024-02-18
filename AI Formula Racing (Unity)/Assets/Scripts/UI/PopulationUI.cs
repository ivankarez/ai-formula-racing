using Ivankarez.AIFR.Common.Utils;
using Ivankarez.AIFR.TrainingAlgorithm;
using Ivankarez.AIFR.UI.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Ivankarez.AIFR.UI
{
    public class PopulationUI : MonoBehaviour
    {
        [SerializeField] private DataView generationDataView;
        [SerializeField] private DataView runtimeDataView;
        [SerializeField] private DataView progressDataView;

        [SerializeField] private DataView currentGenBestFitness;
        [SerializeField] private DataView currentGenBestTime;
        [SerializeField] private DataView currentGenAverageFitness;

        [SerializeField] private DataView prevGenBestFitness;
        [SerializeField] private DataView prevGenBestTime;
        [SerializeField] private DataView prevGenAverageFitness;

        [SerializeField] private GeneticAlgorithm geneticAlgorithm;
        [SerializeField] private TrainingSceneManager sceneManager;

        private float runtime;
        private int populationCount;
        private readonly List<TrainingDriver> runningDrivers = new List<TrainingDriver>();
        private PopulationStats currentPopulationStats;

        private void Awake()
        {
            Check.ArgumentNotNull(generationDataView, nameof(generationDataView));
            Check.ArgumentNotNull(runtimeDataView, nameof(runtimeDataView));
            Check.ArgumentNotNull(progressDataView, nameof(progressDataView));
            Check.ArgumentNotNull(geneticAlgorithm, nameof(geneticAlgorithm));
            Check.ArgumentNotNull(sceneManager, nameof(sceneManager));

            sceneManager.OnDriverStarted.AddListener((driver) =>
            {
                runningDrivers.Add(driver);
                Debug.Log("Driver Started");
            });
            sceneManager.OnDriverFinished.AddListener((driver) =>
            {
                currentPopulationStats.EpisodeCount++;
                currentPopulationStats.SumFitness += driver.DistanceTravelled;
                currentGenAverageFitness.Value = $"{currentPopulationStats.AverageFitness:f2}";
                UpdateProgressDataView();
                runningDrivers.Remove(driver);
                Debug.Log("Driver Finished");
            });

            geneticAlgorithm.OnNewGenerationStarted.AddListener((population) =>
            {
                generationDataView.Value = geneticAlgorithm.Generation.ToString();
                populationCount = population.Count;
                currentPopulationStats = new PopulationStats { Generation = geneticAlgorithm.Generation };
                UpdateProgressDataView();
                Debug.Log("New Generation Started");
            });

            geneticAlgorithm.OnGenerationFinished.AddListener((population) =>
            {
                prevGenBestFitness.Value = $"{currentPopulationStats.BestFitness:f2}";
                prevGenBestTime.Value = $"{currentPopulationStats.BestRuntime:f0}s";
                prevGenAverageFitness.Value = $"{currentPopulationStats.AverageFitness:f2}";
                currentPopulationStats = null;
                Debug.Log("Generation Finished");
            });
        }

        private void Update()
        {
            runtime += (Time.deltaTime / 60);
            runtimeDataView.Value = $"{(int)(runtime / 60):D2}:{(int)(runtime % 60):D2}";

            foreach (var driver in runningDrivers)
            {
                if (driver.DistanceTravelled > currentPopulationStats.BestFitness)
                {
                    currentPopulationStats.BestFitness = driver.DistanceTravelled;
                    currentPopulationStats.BestRuntime = driver.TimeAlive;
                    currentGenBestFitness.Value = $"{currentPopulationStats.BestFitness:f2}";
                    currentGenBestTime.Value = $"{currentPopulationStats.BestRuntime:f0}s";
                }
            }
        }

        private void UpdateProgressDataView()
        {
            progressDataView.Value = $"{currentPopulationStats.EpisodeCount}/{populationCount}";
        }
    }

    public class PopulationStats
    {
        public long Generation { get; set; }
        public float BestFitness { get; set; }
        public float BestRuntime { get; set; }
        public float SumFitness { get; set; }
        public int EpisodeCount { get; set; }
        public float AverageFitness => SumFitness / EpisodeCount;
    }
}
