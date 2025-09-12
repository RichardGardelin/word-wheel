using System.Collections.Generic;
using System.Linq;
using WordWheel.Models;
using WordWheel.Services;
using Xunit;

namespace WordWheel.Tests;

public class RandomizerServiceTests
{
    private static Word MakeWord(string character, string pinyin, string english, List<string> pos)
    {
        return new Word
        {
            Character = character,
            Pinyin = pinyin,
            English = english,
            Pos = pos
        };
    }

    [Fact]
    public void PickWordsByPOS_WithRepeats_ReturnsExpectedCount()
    {
        var service = new RandomizerService();

        var words = new List<Word>
        {
            MakeWord("跑", "pǎo", "run", ["Verb"]),
            MakeWord("跳", "tiào", "jump", ["Verb"]),
            MakeWord("狗", "gǒu", "dog", ["Noun"])
        };

        var filter = new WordFilter
        {
            WordRepeats = true,
            PosCounts = new Dictionary<string, int>
            {
                { "Verb", 2 },
                { "Noun", 1 }
            }
        };

        var result = service.PickWordsByPOS(words, filter);

        Assert.Equal(3, result.Count);
        Assert.Equal(2, result.Count(w => w.PickedForPOS == "Verb"));
        Assert.Equal(1, result.Count(w => w.PickedForPOS == "Noun"));
    }

    [Fact]
    public void PickWordsByPOS_WithoutRepeats_ShufflesAndDequeues()
    {
        var service = new RandomizerService();

        var words = new List<Word>
        {
            MakeWord("跑", "pǎo", "run", ["Verb"]),
            MakeWord("跳", "tiào", "jump", ["Verb"]),
            MakeWord("狗", "gǒu", "dog", ["Noun"])
        };

        var filter = new WordFilter
        {
            WordRepeats = false,
            PosCounts = new Dictionary<string, int>
            {
                { "Verb", 2 },
                { "Noun", 1 }
            }
        };

        var result = service.PickWordsByPOS(words, filter);

        Assert.Equal(3, result.Count);
        Assert.All(result, w => Assert.NotNull(w.Word));
    }

    [Fact]
    public void PickWordsByPOS_Repeats_HandlesNotEnoughWords()
    {
        var service = new RandomizerService();

        var words = new List<Word>
        {
            MakeWord("跑", "pǎo", "run", ["Verb"])
        };

        var filter = new WordFilter
        {
            WordRepeats = true,
            PosCounts = new Dictionary<string, int>
            {
                { "Verb", 3 }
            }
        };

        var result = service.PickWordsByPOS(words, filter);

        Assert.Equal(3, result.Count);
        Assert.All(result, w => Assert.Equal("Verb", w.PickedForPOS));
    }

    [Fact]
    public void PickWordsByPOS_WithoutRepeats_RefillsWhenQueueEmpty()
    {
        var service = new RandomizerService();

        var words = new List<Word>
        {
            MakeWord("跑", "pǎo", "run", ["Verb"]),
            MakeWord("跳", "tiào", "jump", ["Verb"])
        };

        var filter = new WordFilter
        {
            WordRepeats = false,
            PosCounts = new Dictionary<string, int>
            {
                { "Verb", 5 } // Forces queue to empty and refill
            }
        };

        var result = service.PickWordsByPOS(words, filter);

        Assert.Equal(5, result.Count);
        Assert.All(result, r => Assert.Equal("Verb", r.PickedForPOS));
    }

    [Fact]
    public void PickWordsByPOS_EmptyPOS_ReturnsAllFromBooks()
    {
        var service = new RandomizerService();

        var words = new List<Word>
        {
            MakeWord("跑", "pǎo", "run", ["Verb"]),
            MakeWord("跳", "tiào", "jump", ["Verb"]),
            MakeWord("狗", "gǒu", "dog", ["Noun"])
        };

        var filter = new WordFilter
        {
            WordRepeats = true,
            PosCounts = new Dictionary<string, int>() // No POS selected
        };

        var result = service.PickWordsByPOS(words, filter);

        Assert.Empty(result); // Since no POS were requested, nothing should be returned
    }

    [Fact]
    public void PickWordsByPOS_CacheInvalidatedWhenFilterChanges()
    {
        var service = new RandomizerService();

        var words = new List<Word>
        {
            MakeWord("跑", "pǎo", "run", ["Verb"]),
            MakeWord("狗", "gǒu", "dog", ["Noun"])
        };

        var filter1 = new WordFilter
        {
            WordRepeats = false,
            PosCounts = new Dictionary<string, int> { { "Verb", 1 } }
        };

        var result1 = service.PickWordsByPOS(words, filter1);
        Assert.Single(result1);

        var filter2 = new WordFilter
        {
            WordRepeats = false,
            PosCounts = new Dictionary<string, int> { { "Noun", 1 } }
        };

        var result2 = service.PickWordsByPOS(words, filter2);
        Assert.Single(result2);
        Assert.Equal("Noun", result2[0].PickedForPOS);
    }
}
