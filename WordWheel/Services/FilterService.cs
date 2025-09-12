using System.Collections.Generic;
using System.Linq;
using WordWheel.Models;

namespace WordWheel.Services;

public static class FilterService
{
    public static List<Word> GetFilteredList(
        Dictionary<string, List<Word>> wordLists,
        WordFilter filter)
    {
        List<Word> filteredList = [];

        foreach (string book in filter.Books)
        {
            if (wordLists.TryGetValue(book, out var words))
            {
                filteredList.AddRange(words);
            }
        }

        // If no POS selected, return all words from selected books
        if (filter.PosCounts.Count > 0)
        {
            filteredList = [.. filteredList.Where(word => word.Pos.Any(p => filter.PosCounts.ContainsKey(p)))];
        }

        return filteredList;
    }
}