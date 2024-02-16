using System.Collections.Generic;

namespace Ivankarez.AIFR.Common.Utils
{
    public static class CollectionExtensions
    {
        public static void EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }
    }
}
