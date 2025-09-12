using WordWheel.Models;
using WordWheel.Services;

namespace WordWheel.Tests;

public class FilterServiceTests
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
    public void GetFilteredList_WithEmptyFilters_ReturnsEmptyList()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [MakeWord("吃", "chi1", "eat", ["Verb"])]
        };

        var filter = new WordFilter(); // No books or POS

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredList_WithEmptyWordLists_ReturnsEmpty()
    {
        var wordLists = new Dictionary<string, List<Word>>();

        var filter = new WordFilter
        {
            Books = ["book1"],
            PosCounts = new Dictionary<string, int> { ["Verb"] = 1 }
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredList_ExcludesNonMatchingPOS()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [MakeWord("猫", "mao1", "cat", ["Noun"])]
        };

        var filter = new WordFilter
        {
            Books = ["book1"],
            PosCounts = new Dictionary<string, int> { ["Verb"] = 1 }
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredList_WithValidBookAndPos_ReturnsMatchingWords()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [
                MakeWord("吃", "chi1", "eat", ["Verb"]),
                MakeWord("猫", "mao1", "cat", ["Noun"])
            ],
            ["book2"] = [
                MakeWord("吃1", "chi1", "eat", ["Verb"]),
                MakeWord("猫1", "mao1", "cat", ["Noun"])
            ]
        };

        var filter = new WordFilter
        {
            Books = ["book1"],
            PosCounts = new Dictionary<string, int> { ["Verb"] = 1 }
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Single(result);
        Assert.Equal("吃", result[0].Character);
    }

    [Fact]
    public void GetFilteredList_WithMultipleMatchingPOS_DoesNotDuplicate()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [MakeWord("学习", "xue2xi2", "study", ["Verb", "Noun"])]
        };

        var filter = new WordFilter
        {
            Books = ["book1"],
            PosCounts = new Dictionary<string, int>
            {
                ["Verb"] = 1,
                ["Noun"] = 1
            }
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Single(result); // Word should not be duplicated
        Assert.Equal("学习", result[0].Character);
    }

    [Fact]
    public void GetFilteredList_WithNonexistentBook_IgnoresIt()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [MakeWord("吃", "chi1", "eat", ["Verb"])]
        };

        var filter = new WordFilter
        {
            Books = ["book2"], // Doesn't exist
            PosCounts = new Dictionary<string, int> { ["Verb"] = 1 }
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredList_WithEmptyPosCounts_ReturnsAllWordsFromBooks()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [
                MakeWord("吃", "chi1", "eat", ["Verb"]),
                MakeWord("猫", "mao1", "cat", ["Noun"])
            ]
        };

        var filter = new WordFilter
        {
            Books = ["book1"],
            PosCounts = new Dictionary<string, int>() // No POS filtering
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetFilteredList_WithMultipleBooks_MergesCorrectly()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [MakeWord("吃", "chi1", "eat", ["Verb"])],
            ["book2"] = [MakeWord("跑", "pao3", "run", ["Verb"])]
        };

        var filter = new WordFilter
        {
            Books = ["book1", "book2"],
            PosCounts = new Dictionary<string, int> { ["Verb"] = 1 }
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Equal(2, result.Count);
    }
}
