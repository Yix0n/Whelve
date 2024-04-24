using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LeaderboardSaver {
    class LeaderboardSaver {

        [System.Serializable]
        public class LBEntity
        {
            public string name;
            public int points;
            public int time; // Zmieniono na int
            public long date;
        }

        private bool isInstanceSaved = false;

        private string filepath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
    
        public static List<LBEntity> leaderboardEntities { get; private set; } = new List<LBEntity>(); 

        public LeaderboardSaver() {
            LeaderboardFromFile();
            isInstanceSaved = false;
        }

        public void AddNewRecord(string name, int points, float time) 
        {
            if (isInstanceSaved) return;

            var newEntity = new LBEntity {
                name = name,
                points = points,
                time = Mathf.RoundToInt(time), 
                date = DateTime.Now.Ticks
            };
            leaderboardEntities.Add(newEntity);

            SortLeaderboard();
            SaveLeaderboard();
        }

        private void SaveLeaderboard()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filepath))
                {
                    writer.WriteLine("["); // Znak początku listy
                    for (int i = 0; i < leaderboardEntities.Count; i++)
                    {
                        writer.WriteLine(SerializeObjectToJson(leaderboardEntities[i]));
                        if (i < leaderboardEntities.Count - 1)
                        {
                            writer.WriteLine(","); // Przecinek pomiędzy obiektami
                        }
                    }
                    writer.WriteLine("]"); // Znak końca listy
                }
                Debug.Log("Zapisano leaderboard do pliku: " + filepath);
            }
            catch (Exception e)
            {
                Debug.LogError("Błąd podczas zapisu leaderboard do pliku: " + e.Message);
            }
        }

        private string SerializeObjectToJson(LBEntity entity)
        {
            return $"{{\"name\":\"{entity.name.Replace("\"", "")}\",\"points\":{entity.points},\"time\":{entity.time},\"date\":{entity.date}}}";
        }

        private void SortLeaderboard()
        {
            leaderboardEntities.RemoveAll(entity => entity == null);

            if (leaderboardEntities.Count > 0)
            {
                leaderboardEntities.Sort((x, y) => y.points.CompareTo(x.points));
            }
        }


        private void LeaderboardFromFile() 
        {
            if(File.Exists(filepath))
            {
                leaderboardEntities.Clear();
                using (StreamReader reader = new StreamReader(filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Contains("[") && !line.Contains("]"))
                        {
                            leaderboardEntities.Add(DeserializeJsonToObject(line));
                        }
                    }
                }
            }
        }


        private LBEntity DeserializeJsonToObject(string json)
        {
            if (json.Trim() == ",")
            {
                return null;
            }
            
            string[] values = json.Split(',');
            if (values.Length == 4)
            {
                try
                {
                    return new LBEntity
                    {
                        name = values[0].Substring(values[0].IndexOf(":") + 2).Replace('"', '\u200B'),
                        points = int.Parse(values[1].Substring(values[1].IndexOf(":") + 1)),
                        time = int.Parse(values[2].Substring(values[2].IndexOf(":") + 1)),
                        date = long.TryParse(values[3].Substring(values[3].IndexOf(":") + 1).Trim().Replace(",", "."), out long dateValue) ? dateValue : 0
                    };
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing JSON: " + e.Message);
                    return null;
                }
            }
            else
            {
                Debug.LogError("Niepoprawny format linii JSON: " + json);
                return null;
            }
        }

        private DateTime GetDateTime(long ticks)
        {
            return new DateTime(ticks);
        }

        public List<LBEntity> GetLeaders(int r = 10)
        {
            int records = r * 2;
            int count = 0;
            List<LBEntity> leaders = new List<LBEntity>();

            foreach(var entity in leaderboardEntities)
            {
                count++;
                leaders.Add(entity);
                if (count >= records) break;
            }

            return leaders;
        }
    }
}
