using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [Serializable]
    public class LeaderboardEntity
    {
        public string name;
        public int points;
    }

    public GameObject leaderboardGameObject;

    private string filePath = Application.persistentDataPath + "/leaderboard.json";

    private List<LeaderboardEntity> leaderboardEntities = new();

    public void AddNewRecord(string name, int points)
    {
        leaderboardEntities.Add(new LeaderboardEntity { name = name, points = points });
        SortLeaderboard();
        SaveLeaderboard();
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardEntities);
        File.WriteAllText(filePath, json);
    }

    private void SortLeaderboard()
    {
        leaderboardEntities.Sort((x, y) => y.points.CompareTo(x.points));
    }

    private void LeaderboardFromFile()
    {
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            leaderboardEntities = JsonUtility.FromJson<List<LeaderboardEntity>>(json);
        }
    }

    public void ShowLeaderboardOnMainMenu(int records = 10)
    {
        LeaderboardFromFile();

        int count = 0;

        foreach (var entity in leaderboardEntities)
        {

            count++;
            if (count >= records)
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
