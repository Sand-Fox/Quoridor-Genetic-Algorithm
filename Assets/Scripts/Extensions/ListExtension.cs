using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    /// <summary>
    /// Get a random element from the list.
    /// </summary>
    public static T GetRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) return default;
        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Get a random element from the list, excluding a given element.
    /// </summary>
    public static T GetRandomExcluding<T>(this IList<T> list, T elementToExclude)
    {
        if (list.Count == 0) return default;
        if (list.Count == 1) return list[0];

        int indexToExclude = list.IndexOf(elementToExclude);
        if (indexToExclude == -1) return list.GetRandom();

        int randomIndex = Random.Range(0, list.Count - 1);
        if (randomIndex >= indexToExclude) randomIndex++;
        return list[randomIndex];
    }

    /// <summary>
    /// Shuffles the list randomly, based on Fisher Yates shuffle
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (list[r], list[i]) = (list[i], list[r]);
        }
    }
}
