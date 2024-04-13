using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public float moveSpeed = 10f;

    public GameObject hpBar;
    public LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if(currentHealth <= 0)
        {
            // gracz zginął
            levelManager.gameOver();
            hpBar.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            // Gracz został trafiony, ale żyje jeszcze
            hpBar.GetComponent<TextMeshProUGUI>().text = $"HP:\n{currentHealth}/{maxHealth}";
        }
    }
}
