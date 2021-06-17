using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900
{
    public static class Extensions
    {
        public static int GetFirstIndexOf<T>(this T[] array, T itemToFind)
        {
            return Array.IndexOf(array, itemToFind);
        }

        public static int GetFirstIndexOf<T>(this T[] array, T itemToFind, int startIndex)
        {
            return Array.IndexOf(array, itemToFind, startIndex);
        }

        public static T[] SubArray<T>(this T[] array, T itemToFind, int startIndex)
        {
            int index = GetFirstIndexOf(array, itemToFind, startIndex);
            T[] result = new T[index];
            Array.Copy(array, result, index);
            return result;
        }

        public static T[] SubArray<T>(this T[] array, T itemToFind)
        {
            return SubArray(array, itemToFind, 0);
        }
    }
}
