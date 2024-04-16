using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasherController : MonoBehaviour
{
    public int damage = 70;
    GameObject player;
    public float walkSpeed = 1f;
    public int points = 1;
    private LevelManager level;
    private AchievementManager achievement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        level = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        achievement = GameObject.Find("AchievementManager").GetComponent<AchievementManager>();
    }

    void Update()
    {
        transform.LookAt(player.transform.position);
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject projec = collision.gameObject;

        Debug.Log(projec.tag);

        if(projec.CompareTag("PlayerProjectile")) 
        {
            Destroy(projec);

            level.AddPoints(points);

            Destroy(transform.gameObject);
        }

        if(projec.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            if(playerController != null)
            {
                playerController.TakeDamage(damage);


                Destroy(transform.gameObject);
            }
        }

        if(projec.CompareTag("qte")) 
        {
            achievement.GetAchievement("qte");
        }
    }
}