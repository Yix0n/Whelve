using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Transform player;
    public GameObject basherPrefab;
    public float spawnInterval = 1;
    float timeSinceSpawn;
    float spawnDistance = 30;
    int points = 0;
    public GameObject pointsCounter;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        timeSinceSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn > spawnInterval)
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

            UpdateUI();
        }

        //TODO: przyśpieszyć resp over time
    }

    void UpdateUI()
    {
        pointsCounter.GetComponent<TextMeshProUGUI>().text = $"Punkt: {points}";
    }

    public void AddPoints(int amount)
    {
        points += amount;
    }
}