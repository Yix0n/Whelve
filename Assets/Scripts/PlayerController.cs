using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // pobierz san kontrolera (poziom)
        float x = Input.GetAxis("Horizontal");
        

        // wylicz docelowy ruch poziomo (lewo/prawo w osi X) mno¿¹c wychylenie kontrolera przez 1 oraz czas ostatniej klatki

        // dodaj do obecnej pozycji jedn¹ jednostke "w prawo" (w osi X)
        Vector3 movement = Vector3.right * x * Time.deltaTime;

        float y = Input.GetAxis("Vertical");
        movement += Vector3.forward * y * Time.deltaTime;

        movement = movement.normalized;

        movement *= moveSpeed;

        transform.position += movement;
    }
}
