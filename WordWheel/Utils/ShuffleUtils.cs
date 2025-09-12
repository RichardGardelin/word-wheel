using System;
using System.Collections.Generic;

namespace WordWheel.Utils;

public static class ShuffleUtils
{
    public static void Shuffle<T>(this IList<T> list, Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}