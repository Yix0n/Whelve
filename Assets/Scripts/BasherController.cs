using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherController : MonoBehaviour
{
    public int damage = 70;
    GameObject player;
    public float walkSpeed = 1f;

    GameObject LevelManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LevelManager = GameObject.Find("LevelManager");
    }

    void Update()
    {
        //patrz si� na gracza
        transform.LookAt(player.transform.position);
        //idz do przodu
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject projec = collision.gameObject;

        if(projec.CompareTag("PlayerProjectile")) 
        {
            LevelManager.GetComponent<LevelManager>().AddPoints(1);

            Destroy(projec);

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