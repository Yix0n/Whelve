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

    GameObject LevelManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LevelManager = GameObject.Find("LevelManager");
    }

    void Update()
    {
        transform.LookAt(player.transform.position);
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject projec = collision.gameObject;

        if(projec.CompareTag("PlayerProjectile")) 
        {
            Destroy(projec);

            LevelManager.GetComponent<LevelManager>().AddPoints(points);

            Destroy(transform.gameObject);
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            if(playerController != null)
            {
                playerController.TakeDamage(damage);


                Destroy(transform.gameObject);
            }
        }
    }
}