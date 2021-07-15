using System.Collections;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static string Print<T>(this List<T> list)
        {
            return string.Join(" | ", list);
        }

        public static List<T> SelectRandom<T>(this List<T> list, int count)
        {
            List<T> selected = new List<T>();
            var rand = new System.Random();
            for (var i = 0; i < count; i++)
            {
                var randomIndex = rand.Next(list.Count);
                selected.Add(list[randomIndex]);
                list.RemoveAt(randomIndex);
            }
            return selected;
        }
    }
}
