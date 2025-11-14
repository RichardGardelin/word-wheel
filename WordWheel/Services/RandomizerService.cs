using System;
using System.Collections.Generic;
using System.Linq;
using WordWheel.Models;
using WordWheel.Utils;

namespace WordWheel.Services;

public class RandomizerService
{
    private readonly Random _random = new();
    private readonly Dictionary<string, Queue<Word>> _shuffledCache = [];
    private WordFilter? _lastUsedFilter;

    public List<RandomizedWord> PickWordsByFilter(List<Word> filteredList, WordFilter filter)
    {
        if (_lastUsedFilter == null || !FilterUtils.FiltersAreEqual(_lastUsedFilter, filter))
        {
            _shuffledCache.Clear();
            _lastUsedFilter = FilterUtils.CloneFilter(filter);
        }

        var pools = BuildPools(filteredList, filter.PosCounts);
        var randomWords = new List<RandomizedWord>();

        if (filter.WordRepeats)
        {
            randomWords.AddRange(PickWithRepeats(pools, filter.PosCounts));
        }
        else
        {
            randomWords.AddRange(PickWithoutRepeats(pools, filter.PosCounts));
        }

        return randomWords;
    }

    private static Dictionary<string, List<Word>> BuildPools(
        List<Word> filteredList,
        Dictionary<string, int> posCounts
    )
    {
        var pools = new Dictionary<string, List<Word>>();

        foreach (string category in posCounts.Keys)
        {
            if (category == "Random")
            {
                pools[category] = [.. filteredList];
            }
            else
            {
                pools[category] = [.. filteredList.Where(word => word.Pos.Contains(category))];
            }
        }

        return pools;
    }

    private List<RandomizedWord> PickWithRepeats(
        Dictionary<string, List<Word>> pools,
        Dictionary<string, int> posCounts
    )
    {
        // Picks words randomly and tries to prevent duplicates within the same call
        var randomWords = new List<RandomizedWord>();

        foreach (var (category, count) in posCounts)
        {
            if (!pools.TryGetValue(category, out var pool) || pool.Count == 0)
                continue;

            var pickedForThisPOS = new HashSet<Word>();

            for (int i = 0; i < count; i++)
            {
                Word selectedWord;

                int attempts = 0;
                int maxAttempts = pool.Count;

                do
                {
                    selectedWord = pool[_random.Next(pool.Count)];
                    attempts++;
                } while (pickedForThisPOS.Contains(selectedWord) && attempts < maxAttempts);

                // If failed to get a unique word, just allow a duplicate
                pickedForThisPOS.Add(selectedWord);

                randomWords.Add(new RandomizedWord(selectedWord, category));
            }
        }
        return randomWords;
    }

    private List<RandomizedWord> PickWithoutRepeats(
        Dictionary<string, List<Word>> pools,
        Dictionary<string, int> posCounts
    )
    {
        var randomWords = new List<RandomizedWord>();

        foreach (var (category, count) in posCounts)
        {
            if (!pools.TryGetValue(category, out var sourceList) || sourceList.Count == 0)
                continue;

            var queue = GetOrRefillQueue(category, sourceList);

            for (int i = 0; i < count; i++)
            {
                if (queue.Count == 0)
                    queue = GetOrRefillQueue(category, sourceList);

                randomWords.Add(new RandomizedWord(queue.Dequeue(), category));
            }
        }

        return randomWords;
    }

    private Queue<Word> GetOrRefillQueue(string category, List<Word> sourceList)
    {
        if (!_shuffledCache.TryGetValue(category, out var queue) || queue.Count == 0)
        {
            ShuffleUtils.Shuffle(sourceList, _random);
            queue = new Queue<Word>(sourceList);
            _shuffledCache[category] = queue;
        }

        return queue;
    }
}
