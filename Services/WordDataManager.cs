using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WordWheel.Models;

namespace WordWheel.Services;

public class WordDataManager
{
    private Dictionary<string, List<Word>> _wordLists = [];
    private Dictionary<string, bool> _isDirty = [];

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

    public List<Word> LoadAllUserWords()
    {
        // TODO Add so each HSK file is using its own list in class field
        List<Word> allWords = [];
        string userPath = GetUserWordFolderPath();

        foreach (var file in Directory.GetFiles(userPath, "*.json"))
        {
            string json = File.ReadAllText(file);

            List<Word>? fileWords = JsonSerializer.Deserialize<List<Word>>(json);

            if (fileWords != null)
            {
                allWords.AddRange(fileWords);
            }
        }

        return allWords;
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