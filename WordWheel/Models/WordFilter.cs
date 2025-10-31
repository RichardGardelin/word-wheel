using System.Collections.Generic;

namespace WordWheel.Models;

public class WordFilter
{
    public HashSet<string> Books { get; set; } = [];
    public HashSet<string> AllLessonsBooks { get; set; } = [];
    public Dictionary<string, HashSet<int>> Lessons { get; set; } = [];
    public Dictionary<string, int> PosCounts { get; set; } = [];
    public bool WordRepeats { get; set; }
}
