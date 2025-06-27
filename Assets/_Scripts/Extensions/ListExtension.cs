using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Extensions
{
    public static class ListExtension
    {
        public static T Draw<T>(this List<T> list)
        {
            if (list.Count is 0) return default;

            int idx = Random.Range(0, list.Count);
            T value = list[idx];
            list.RemoveAt(idx);
            return value;
        }

        public static T GetOne<T>(this List<T> list)
        {
            if (list.Count is 0) return default;

            int idx = Random.Range(0, list.Count);
            T value = list[idx];
            return value;
        }
    }
}