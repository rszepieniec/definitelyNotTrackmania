using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class RunRecord
{
    public string id;          // unikalne ID (timestamp) — używane do podświetlenia "ostatniego runa"
    public string playerName;
    public string dateIso;     // ISO 8601 timestamp, łatwo sortować / wyświetlić
    public int coinsEarned;
    public string carName;
    public int deliveryCount;
    public int avgReward;
    public int maxReward;

    public DateTime GetDate()
    {
        return DateTime.TryParse(dateIso, out var d) ? d : DateTime.MinValue;
    }
}

[Serializable]
public class RunRecordsDatabase
{
    public List<RunRecord> runs = new List<RunRecord>();
}

public static class RunRecordsStorage
{
    private const string FileName = "run_records.json";

    private static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

    public static RunRecordsDatabase Load()
    {
        if (!File.Exists(FilePath))
        {
            return new RunRecordsDatabase();
        }
        try
        {
            string json = File.ReadAllText(FilePath);
            var db = JsonUtility.FromJson<RunRecordsDatabase>(json);
            return db ?? new RunRecordsDatabase();
        }
        catch (Exception e)
        {
            Debug.LogError($"RunRecordsStorage: failed to load: {e.Message}");
            return new RunRecordsDatabase();
        }
    }

    public static void Save(RunRecordsDatabase db)
    {
        try
        {
            string json = JsonUtility.ToJson(db, true);
            File.WriteAllText(FilePath, json);
            Debug.Log($"RunRecordsStorage: saved {db.runs.Count} runs to {FilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"RunRecordsStorage: failed to save: {e.Message}");
        }
    }

    public static void Append(RunRecord record)
    {
        var db = Load();
        db.runs.Add(record);
        Save(db);
    }
}
