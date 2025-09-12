using System.Collections.Generic;

namespace WordWheel.Models;

public class WordFilter
{
    public List<string> Books { get; set; } = [];
    public Dictionary<string, int> PosCounts { get; set; } = [];
    public bool WordRepeats { get; set; }
}