using System.Collections.Generic;
using System.Linq;
using WordWheel.Models;

namespace WordWheel.Utils;

public static class FilterUtils
{
    public static bool FiltersAreEqual(WordFilter a, WordFilter b)
    {
        if (a is null || b is null)
            return false;

        return a.WordRepeats == b.WordRepeats
            && a.Books.SetEquals(b.Books)
            && a.AllLessonsBooks.SetEquals(b.AllLessonsBooks)
            && a.Lessons.Count == b.Lessons.Count
            && a.Lessons.All(kvp =>
                b.Lessons.TryGetValue(kvp.Key, out var value) && value.SetEquals(kvp.Value)
            )
            && a.PosCounts.Count == b.PosCounts.Count
            && a.PosCounts.All(kvp =>
                b.PosCounts.TryGetValue(kvp.Key, out var value) && value == kvp.Value
            );
    }

    public static WordFilter CloneFilter(WordFilter original)
    {
        return new WordFilter
        {
            WordRepeats = original.WordRepeats,
            Books = [.. original.Books],
            AllLessonsBooks = [.. original.AllLessonsBooks],
            Lessons = original.Lessons.ToDictionary(
                kvp => kvp.Key,
                kvp => new HashSet<int>(kvp.Value)
            ),
            PosCounts = new Dictionary<string, int>(original.PosCounts),
        };
    }
}
