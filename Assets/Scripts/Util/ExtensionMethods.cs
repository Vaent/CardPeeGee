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
    }
}
