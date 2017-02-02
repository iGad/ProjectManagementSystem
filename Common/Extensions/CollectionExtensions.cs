using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> addingItems)
        {
            foreach (var item in addingItems)
            {
                collection.Add(item);
            }
        }
    }
}
