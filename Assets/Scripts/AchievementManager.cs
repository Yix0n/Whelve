using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public GameObject levelManager;
    public GameObject achievementPlace; // to co ma się przesunąć
    public GameObject achievementName;
    public GameObject achievementDesc;
    
    private LevelManager level;

    // Start is called before the first frame update
    void Start()
    {
        level = levelManager.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(level.points == 1)
        {
            ShowAchievement("Woah!", "To twój pierwszy punkt!");
        }
    }

    private void ShowAchievement(string name, string shortDesc)
    {
        // TODO: Dodać by zjechało to w dół, potem spowrotem do góry
        achievementName.GetComponent<TextMeshProUGUI>().text = name;
        achievementDesc.GetComponent<TextMeshProUGUI>().text = shortDesc;

        achievementPlace.SetActive(true);

        Invoke("HideAchievement", 5f);
    }

    private void HideAchievement()
    {
        achievementName.GetComponent<TextMeshProUGUI>().text = "";
        achievementDesc.GetComponent<TextMeshProUGUI>().text = "";

        achievementPlace.SetActive(false);
    }
}
