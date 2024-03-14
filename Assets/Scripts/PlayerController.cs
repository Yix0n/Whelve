using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // pobierz san kontrolera (poziom)
        float x = Input.GetAxisRaw("Horizontal");

        Vector3 movement = Vector3.right * x;

        float y = Input.GetAxisRaw("Vertical");
        movement += Vector3.forward * y;

        movement = movement.normalized;

        movement *= Time.deltaTime;

        movement *= moveSpeed;

        transform.position += movement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // logika œmierci gracza
            Debug.Log("Gracz trafiony");
        }
    }
}
