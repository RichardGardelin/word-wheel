using System.Collections.Generic;

namespace WordWheel.Models;

public class Word
{
    public required string Character { get; set; }
    public required string Pinyin { get; set; }
    public required List<string> Pos { get; set; }
    public required string English { get; set; }
}
