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

    public List<RandomizedWord> PickWordsByPOS(List<Word> wordList, WordFilter filter)
    {
        if (_lastUsedFilter == null || !FilterUtils.FiltersAreEqual(_lastUsedFilter, filter))
        {
            _shuffledCache.Clear();
            _lastUsedFilter = FilterUtils.CloneFilter(filter);
        }

        var posLists = BuildPOSLists(wordList, filter.PosCounts);
        var randomWords = new List<RandomizedWord>();

        if (filter.WordRepeats)
        {
            randomWords.AddRange(PickWithRepeats(posLists, filter.PosCounts));
        }
        else
        {
            randomWords.AddRange(PickWithoutRepeats(posLists, filter.PosCounts));
        }

        return randomWords;
    }

    private static Dictionary<string, List<Word>> BuildPOSLists(
        List<Word> wordList,
        Dictionary<string, int> posCounts)
    {
        var posLists = new Dictionary<string, List<Word>>();

        foreach (string pos in posCounts.Keys)
        {
            posLists[pos] = [.. wordList.Where(word => word.Pos.Contains(pos))];
        }

        return posLists;
    }

    private List<RandomizedWord> PickWithRepeats(
        Dictionary<string, List<Word>> posLists,
        Dictionary<string, int> posCounts)
    {
        // Picks words randomly and tries to prevent duplicates within the same call
        var randomWords = new List<RandomizedWord>();

        foreach (var (pos, count) in posCounts)
        {
            if (!posLists.TryGetValue(pos, out var pool) || pool.Count == 0)
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
                }
                while (pickedForThisPOS.Contains(selectedWord) && attempts < maxAttempts);

                // If failed to get a unique word, just allow a duplicate
                pickedForThisPOS.Add(selectedWord);

                randomWords.Add(new RandomizedWord(selectedWord, pos));
            }
        }
        return randomWords;
    }

    private List<RandomizedWord> PickWithoutRepeats(
        Dictionary<string, List<Word>> posLists,
        Dictionary<string, int> posCounts
        )
    {
        var randomWords = new List<RandomizedWord>();

        foreach (var (pos, count) in posCounts)
        {
            if (!posLists.TryGetValue(pos, out var sourceList) || sourceList.Count == 0)
                continue;

            var queue = GetOrRefillQueue(pos, sourceList);

            for (int i = 0; i < count; i++)
            {
                if (queue.Count == 0)
                    queue = GetOrRefillQueue(pos, sourceList);

                randomWords.Add(new RandomizedWord(queue.Dequeue(), pos));
            }
        }

        return randomWords;
    }

    private Queue<Word> GetOrRefillQueue(string pos, List<Word> sourceList)
    {
        if (!_shuffledCache.TryGetValue(pos, out var queue) || queue.Count == 0)
        {
            ShuffleUtils.Shuffle(sourceList, _random);
            queue = new Queue<Word>(sourceList);
            _shuffledCache[pos] = queue;
        }

        return queue;
    }
}