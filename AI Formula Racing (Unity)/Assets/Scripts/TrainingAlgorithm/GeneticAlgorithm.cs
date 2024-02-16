using Ivankarez.AIFR.Common.Utils;
using Ivankarez.NeuralNetworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ivankarez.AIFR.TrainingAlgorithm
{
    public class GeneticAlgorithm : MonoBehaviour
    {
        private static readonly System.Random random = new();

        [SerializeField] private AIFRSettingsProvider settingsProvider;
        [SerializeField] private NeuralNetworkProvider neuralNetworkProvider;
        [SerializeField] private SelectionAlgorithm selection;

        private readonly List<Individual> population = new();
        private long generation = 0;
        private long individuals = 0;

        private void Awake()
        {
            Check.ArgumentNotNull(settingsProvider);
            Check.ArgumentNotNull(neuralNetworkProvider);
            Check.ArgumentNotNull(selection);
        }

        private void Start()
        {
            CreateFirstGeneration();
        }

        private void CreateFirstGeneration()
        {
            population.Clear();
            var setting = settingsProvider.Settings;
            for (int i = 0; i < setting.PopultaionSize; i++)
            {
                var embeddingWeights = neuralNetworkProvider.CreateImageEmbeddingModel().GetParametersFlat();
                var drivingWeights = neuralNetworkProvider.CreateDrivingModel().GetParametersFlat();
                var individual = new Individual(individuals++, embeddingWeights, drivingWeights, generation, DateTime.Now);
                population.Add(individual);
            }
        }

        public IEnumerable<Individual> GetIndividualsToTest()
        {
            return population.Where(i => !i.Fitness.HasValue);
        }

        public void CreateNextGeneration(Action callback)
        {
            if (population.All(i => !i.Fitness.HasValue))
            {
                throw new InvalidOperationException("All individuals must have fitness value before creating next generation");
            }
            Debug.Log($"Result of gen {generation}: {population.OrderByDescending(i => i.Fitness).First().Fitness:f2}");

            generation++;
            var setting = settingsProvider.Settings;
            var newPopulation = CreateNewPopulation();
            population.Clear();
            population.AddRange(newPopulation);
            callback();
        }

        private List<Individual> CreateNewPopulation()
        {
            var newPopulation = new List<Individual>();
            var settings = settingsProvider.Settings;
            var survivors = selection.DoSelection(population).ToList();
            while (newPopulation.Count < settings.PopultaionSize)
            {
                var parents = random.Shuffle(survivors).ToList();
                for (int i = 0; i < parents.Count - 1; i += 2)
                {
                    var parent1 = parents[i];
                    var parent2 = parents[i + 1];
                    var childEmbeddingWeights = Crossover(parent1.EmbeddingWeights, parent2.EmbeddingWeights);
                    var childDrivingWeights = Crossover(parent1.DrivingNetworkWeights, parent2.DrivingNetworkWeights);
                    Mutate(childEmbeddingWeights);
                    Mutate(childDrivingWeights);
                    var child = new Individual(individuals++, childEmbeddingWeights, childDrivingWeights, generation, DateTime.Now);
                    newPopulation.Add(child);
                }
            }
            return newPopulation;
        }

        private void Mutate(float[] values)
        {
            var settings = settingsProvider.Settings;
            for (int i = 0; i < values.Length; i++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < settings.MutationRate)
                {
                    values[i] += random.NextGaussianFloat(stdDev: 0.15f);
                }
            }
        }

        private float[] Crossover(float[] parent1, float[] parent2)
        {
            var child = new float[parent1.Length];
            var crossoverPoint1 = UnityEngine.Random.Range(0, parent1.Length);
            var crossoverPoint2 = UnityEngine.Random.Range(crossoverPoint1, parent1.Length);

            for (int i = 0; i < parent1.Length; i++)
            {
                if (i < crossoverPoint1 || i > crossoverPoint2)
                {
                    child[i] = parent1[i];
                }
                else
                {
                    child[i] = parent2[i];
                }
            }

            return child;
        }
    }
}
