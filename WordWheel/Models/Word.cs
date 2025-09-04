using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WordWheel.Models;

public class Word
{
    [JsonPropertyName("character")]
    public required string Character { get; set; }

    [JsonPropertyName("pinyin")]
    public required string Pinyin { get; set; }

    [JsonPropertyName("pos")]
    public required List<string> Pos { get; set; }

    [JsonPropertyName("english")]
    public required string English { get; set; }
}
