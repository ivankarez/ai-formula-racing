using System;
using System.Collections.Generic;
using System.Linq;

namespace Ivankarez.AIFR.Common.Utils
{
    public static class RandomUtils
    {
        public static T Select<T>(this Random random, IEnumerable<T> items)
        {
            return items.ElementAt(random.Next(items.Count()));
        }

        public static IEnumerable<T> Select<T>(this Random random, IEnumerable<T> items, int count)
        {
            return items.OrderBy(x => random.Next()).Take(count);
        }

        public static IEnumerable<T> SelectByWeight<T>(this Random random, IEnumerable<T> items, int count, Func<T, float> weightFunc)
        {
            var itemsList = items.ToList();
            if (itemsList.Count == 0)
            {
                return new List<T>();
            }

            var weights = itemsList.Select(weightFunc).ToList();
            var totalWeight = weights.Sum();
            if (totalWeight == 0)
            {
                return Select(random, items, count);
            }

            var selectedItems = new List<T>();
            for (var i = 0; i < count; i++)
            {
                var randomValue = random.NextDouble() * totalWeight;
                var currentWeight = 0f;
                for (var j = 0; j < itemsList.Count; j++)
                {
                    currentWeight += weights[j];
                    if (currentWeight >= randomValue)
                    {
                        selectedItems.Add(itemsList[j]);
                        break;
                    }
                }
            }

            return selectedItems;
        }

        public static IEnumerable<T> Shuffle<T>(this Random random, IEnumerable<T> items)
        {
            return items.OrderBy(x => random.Next());
        }

        public static bool Chance(this Random random, float chance)
        {
            return random.NextDouble() < chance;
        }

        public static float NextGaussianFloat(this Random random, float mean = 0f, float stdDev = 1f)
        {
            var u1 = 1.0 - random.NextDouble();
            var u2 = 1.0 - random.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return (float)(mean + stdDev * randStdNormal);
        }
    }
}
