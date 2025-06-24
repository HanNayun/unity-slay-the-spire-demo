using System.Collections.Generic;

public static class ListExtension
{
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count is 0) return default;

        int idx = UnityEngine.Random.Range(0, list.Count);
        var value = list[idx];
        list.RemoveAt(idx);
        return value;
    }
}