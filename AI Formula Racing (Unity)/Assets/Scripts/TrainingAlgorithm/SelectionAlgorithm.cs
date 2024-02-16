using Ivankarez.AIFR.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ivankarez.AIFR.TrainingAlgorithm
{
    public class SelectionAlgorithm : MonoBehaviour
    {
        private static readonly System.Random Random = new();

        [SerializeField] private AIFRSettingsProvider settingsProvider;

        public IEnumerable<Individual> DoSelection(IEnumerable<Individual> originalPopulation)
        {
            var remainingPopulation = new List<Individual>(originalPopulation);
            var settings = settingsProvider.Settings;

            for (int i = 0; i < settings.SurvivorCount; i++)
            {
                var useTorunamentSelection = Random.Chance(settings.TournamentSelectionChance);
                var selected = useTorunamentSelection
                    ? SelectIndividualByTournament(remainingPopulation)
                    : SelectIndividualByRouletteWheel(remainingPopulation);
                remainingPopulation.Remove(selected);
                yield return selected;
            }
        }

        private Individual SelectIndividualByRouletteWheel(List<Individual> population)
        {
            return Random.SelectByWeight(population, 1, (i) => i.Fitness ?? -1f).First();
        }

        private Individual SelectIndividualByTournament(List<Individual> population)
        {
            var tournamentParticipants = Random.Select(population, settingsProvider.Settings.TournamentSize);
            return tournamentParticipants.OrderByDescending(individual => individual.Fitness).First();
        }
    }
}
