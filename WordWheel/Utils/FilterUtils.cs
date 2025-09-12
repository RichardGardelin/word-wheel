using System.Collections.Generic;
using System.Linq;
using WordWheel.Models;

namespace WordWheel.Utils;

public static class FilterUtils
{
    public static bool FiltersAreEqual(WordFilter a, WordFilter b)
    {
        if (a.WordRepeats != b.WordRepeats)
            return false;

        if (!a.Books.SequenceEqual(b.Books))
            return false;

        if (a.PosCounts.Count != b.PosCounts.Count)
            return false;

        foreach (var (pos, count) in a.PosCounts)
        {
            if (!b.PosCounts.TryGetValue(pos, out var otherCount) || otherCount != count)
                return false;
        }

        return true;
    }

    public static WordFilter CloneFilter(WordFilter original)
    {
        return new WordFilter
        {
            WordRepeats = original.WordRepeats,
            Books = [.. original.Books],
            PosCounts = new Dictionary<string, int>(original.PosCounts)
        };
    }
}
