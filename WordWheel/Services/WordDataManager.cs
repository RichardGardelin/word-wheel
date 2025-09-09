using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WordWheel.Models;

namespace WordWheel.Services;

public class WordDataManager
{
    private readonly Dictionary<string, List<Word>> _wordLists = [];
    private readonly Dictionary<string, bool> _isDirty = [];
    private readonly List<Word> _filteredCache = [];

    public Word GetRandomWordByPos(WordFilter filter)
    {
        _filteredCache.Clear();
        _filteredCache.AddRange(FilterService.GetFilteredList(_wordLists, filter));

        //Temp return, TODO
        return _filteredCache[0];
    }

    public void EnsureUserWordFilesExist()
    {
        string userPath = GetUserWordFolderPath();
        string defaultPath = GetDefaultWordFolderPath();

        if (!Directory.Exists(userPath))
        {
            Directory.CreateDirectory(userPath);
        }

        foreach (var file in Directory.GetFiles(defaultPath, "*.json"))
        {
            string targetPath = Path.Combine(userPath, Path.GetFileName(file));

            if (!File.Exists(targetPath))
            {
                File.Copy(file, targetPath);
            }
        }
    }

    public void LoadAllUserWords()
    {
        // Load all user word lists and mark them as not dirty
        string userPath = GetUserWordFolderPath();

        foreach (var file in Directory.GetFiles(userPath, "*.json"))
        {
            string fileName = Path.GetFileName(file);
            string json = File.ReadAllText(file);
            List<Word> fileWords = JsonSerializer.Deserialize<List<Word>>(json) ?? [];

            _wordLists[fileName] = fileWords;
            _isDirty[fileName] = false;
        }
    }

    private static string GetUserWordFolderPath()
    {
        string userPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(userPath, "WordWheel");
    }

    private static string GetDefaultWordFolderPath()
    {
        string defaultPath = AppContext.BaseDirectory;
        return Path.Combine(defaultPath, "Words");
    }
}