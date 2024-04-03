using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 targetDirection = new Vector3(x, 0, y);
        Vector3 targetPosition = transform.position + targetDirection;
        if(targetDirection.magnitude > Mathf.Epsilon)
        {
            transform.LookAt(targetPosition);
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // logika œmierci gracza
            Debug.Log("Gracz trafiony");
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameObject();
        }
    }
}
