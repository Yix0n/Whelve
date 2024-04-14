using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public float moveSpeed = 10f;

    public GameObject hpBar;
    public LevelManager levelManager;

    private bool isImmune = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hpBar.GetComponent<TextMeshProUGUI>().text = $"HP:\n{currentHealth}/{maxHealth}";
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

        // abilities

        if(Input.GetKeyDown(KeyCode.Z))
        {
            //dash
            Dash();

        } else if(Input.GetKeyDown(KeyCode.X))
        {
            
        } else if(Input.GetKeyDown(KeyCode.C))
        {
            
        }
    }

    public void TakeDamage(int damage)
    {

        if(isImmune) return;
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

    private void Dash(){
        ToggleImmune();
        float dashDistance = 20f;
        Vector3 dashTarget = transform.position + transform.forward * dashDistance;

        transform.position = dashTarget;

        RaycastHit[] hits = Physics.RaycastAll(transform.position - transform.forward * dashDistance, transform.forward, dashDistance);

        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                levelManager.GetComponent<LevelManager>().AddPoints(hit.collider.gameObject.GetComponent<BasherController>().points);
            }
        }

        Invoke("ToggleImmune", 0.5f);
    }

    private void ToggleImmune()
    {
        isImmune = !isImmune;
    }
}
