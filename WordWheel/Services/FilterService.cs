using System.Collections.Generic;
using System.Linq;
using WordWheel.Models;

namespace WordWheel.Services;

public static class FilterService
{
    public static List<Word> GetFilteredList(
        Dictionary<string, List<Word>> wordLists,
        WordFilter filter
    )
    {
        List<Word> filteredList = [];

        foreach (string book in filter.Books)
        {
            if (!wordLists.TryGetValue(ToFileName(book), out var words))
                continue;

            if (filter.AllLessonsBooks.Contains(book))
            {
                filteredList.AddRange(words);
                continue;
            }

            if (filter.Lessons.TryGetValue(book, out var lessonSet))
            {
                filteredList.AddRange(
                    words.Where(word => word.Lesson is int l && lessonSet.Contains(l))
                );
            }
        }

        // If no POS selected or no RandomWords, return all words from selected books
        if (filter.PosCounts.Count > 0 && !filter.PosCounts.ContainsKey("Random"))
        {
            filteredList =
            [
                .. filteredList.Where(word => word.Pos.Any(p => filter.PosCounts.ContainsKey(p))),
            ];
        }

        return filteredList;
    }

    private static string ToFileName(string name)
    {
        // Converts UI name into file data name
        return $"{name.Replace(" ", string.Empty)}.json";
    }
}
