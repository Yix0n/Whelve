using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Transform player;
    public GameObject basherPrefab;
    public float spawnInterval = 1;

    public float spawnIncreaseRate = 0.1f;
    public float timeToIncreaseSpawn = 30f;
    private float timeSinceSpawn;
    private float spawnDistance = 30;
    public int points = 0;
    public GameObject pointsCounter;
    public GameObject timeCounter;
    public GameObject gameOverScreen;
    public GameObject gameOverPoints;
    public GameObject gameOverTimer;
    public float levelTime = 0f;
    public float timeSinceGameStart = 0f;
    private bool doEnemySpawn = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        timeSinceSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        timeSinceGameStart += Time.deltaTime;

        if(timeSinceGameStart > timeToIncreaseSpawn) {
            if (spawnInterval > 0)
            {
                spawnInterval -= spawnIncreaseRate;
            }
            timeSinceGameStart = 0f;
        }

        if (timeSinceSpawn > spawnInterval && doEnemySpawn)
        {
            Vector2 random = Random.insideUnitCircle.normalized;

            Vector3 randomPosition = new Vector3(random.x, 0, random.y);


            randomPosition *= spawnDistance;

            randomPosition += player.position;

            if (!Physics.CheckSphere(new Vector3(randomPosition.x, 1, randomPosition.z), 0.5f))
            {

                Instantiate(basherPrefab, randomPosition, Quaternion.identity);

                timeSinceSpawn = 0;
            }
            
        }

        if(doEnemySpawn)
        {
            levelTime += Time.deltaTime;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        pointsCounter.GetComponent<TextMeshProUGUI>().text = $"Punkt: {points}";
        timeCounter.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(levelTime).ToString();
    }

    public void AddPoints(int amount)
    {
        points += amount;
    }

    public void gameOver()
    {
        player.GetComponent<PlayerController>().enabled = false;
        doEnemySpawn = false;
        player.transform.Find("object_head").GetComponent<WeaponController>().enabled = false;

        gameOverPoints.GetComponent<TextMeshProUGUI>().text = $"Punkty: {points}";
        gameOverTimer.GetComponent<TextMeshProUGUI>().text = $"Czas: {Mathf.Floor(levelTime)}";

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<BasherController>().enabled = false;
        }

        gameOverScreen.SetActive(true);
        pointsCounter.SetActive(false);
        timeCounter.SetActive(false);        
    }
}

