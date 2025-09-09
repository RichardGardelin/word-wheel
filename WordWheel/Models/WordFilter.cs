using System.Collections.Generic;

namespace WordWheel.Models;

public class WordFilter
{
    public List<string> Books { get; set; } = [];
    public List<string> Pos { get; set; } = [];
}