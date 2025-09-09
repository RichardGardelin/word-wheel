using WordWheel.Models;
using WordWheel.Services;

namespace WordWheel.Tests;

public class FilterServiceTests
{
    [Fact]
    public void GetFilteredList_WithEmptyFilters_ReturnsEmptyList()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] =
            [
                new Word { Character = "吃", Pinyin = "chi1", English = "eat", Pos = ["Verb"] }
            ]
        };

        var filter = new WordFilter(); // no books or pos

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
            Pos = ["Verb"]
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredList_ExcludesNonMatchingPOS()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] = [
                new Word { Character = "猫", Pinyin = "mao1", English = "cat", Pos = ["Noun"] }
            ]
        };

        var filter = new WordFilter
        {
            Books = ["book1"],
            Pos = ["Verb"]
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredList_WithValidBookAndPos_ReturnsMatchingWords()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] =
            [
                new Word { Character = "吃", Pinyin = "chi1", English = "eat", Pos = ["Verb"] },
                new Word { Character = "猫", Pinyin = "mao1", English = "cat", Pos = ["Noun"] }
            ],
            ["book2"] =
            [
                new Word { Character = "吃1", Pinyin = "chi1", English = "eat", Pos = ["Verb"] },
                new Word { Character = "猫1", Pinyin = "mao1", English = "cat", Pos = ["Noun"] }
            ]
        };

        var filter = new WordFilter
        {
            Books = ["book1"],
            Pos = ["Verb"]
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
            ["book1"] =
            [
                new Word { Character = "学习", Pinyin = "xue2xi2", English = "study", Pos = ["Verb", "Noun"] }
            ]
        };

        var filter = new WordFilter
        {
            Books = ["book1"],
            Pos = ["Verb", "Noun"]
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Single(result); // Not duplicated
    }

    [Fact]
    public void GetFilteredList_WithNonexistentBook_IgnoresIt()
    {
        var wordLists = new Dictionary<string, List<Word>>
        {
            ["book1"] =
            [
                new Word { Character = "吃", Pinyin = "chi1", English = "eat", Pos = ["Verb"] }
            ]
        };

        var filter = new WordFilter
        {
            Books = ["book2"], // doesn't exist
            Pos = ["Verb"]
        };

        var result = FilterService.GetFilteredList(wordLists, filter);

        Assert.Empty(result);
    }
}
