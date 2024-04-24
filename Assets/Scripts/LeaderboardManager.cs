using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeaderboardSaver;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    private LeaderboardSaver.LeaderboardSaver lbm;
    public GameObject leaderboardGO;
    
    public GameObject LevelManagerGO;
    private LevelManager levelManager;
    public TMP_InputField nameInput;
    // Start is called before the first frame update
    void Start()
    {
        lbm = new();

        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            DisplayLeaders();
        }
    }

    public void SubmitScore()
    {
        levelManager = LevelManagerGO.GetComponent<LevelManager>();

        string name = nameInput.text;
        int points = levelManager.points;
        float time = levelManager.levelTime;

        lbm.AddNewRecord(name, points, time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayLeaders(){
        List<LeaderboardSaver.LeaderboardSaver.LBEntity> leaders = lbm.GetLeaders(10);

        TextMeshProUGUI lbtextspace = leaderboardGO.GetComponent<TextMeshProUGUI>();

        lbtextspace.text = "";

        int i = 0;
        foreach (var leader in leaders)
        {

            if (leader != null && leader.name != null)
            {
                i++;
                string leaderboardEntity = $"{i}. {leader.name} - {leader.points} pkt - {leader.time} s\n";
                lbtextspace.text += leaderboardEntity;
            } else continue;
        }
    }
}
