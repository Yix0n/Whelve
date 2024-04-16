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
    public WeaponController weaponController;
    private bool isImmune = false;
    private float overcloakTime = 8f;
    private float originalRoF;
    private float originalSpeed;
    public GameObject AbilityDisplay;
    public float dashCooldown = 8f;
    private float timeSinceDash;
    public float shockwaveCooldown = 20f;
    private float timeSinceShockwave;
    public float overcloakCooldown = 15f;
    private float timeSinceOvercloak;
    public GameObject qtePrefab;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceDash = Time.time - dashCooldown;
        timeSinceShockwave = Time.time - shockwaveCooldown;
        timeSinceOvercloak = Time.time - overcloakCooldown;

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
            if(Time.time - timeSinceDash >= dashCooldown){
                Dash();
                timeSinceDash = Time.time;
            } else {
                Debug.Log("Dash jest na cooldownie");
            }
        }
        
        if(Input.GetKeyDown(KeyCode.X))
        {
            if(Time.time - timeSinceShockwave >= shockwaveCooldown){
                Shockwave();
                timeSinceShockwave = Time.time;
            } else {
                Debug.Log("Shockwave jest na cooldownie");
            }
            
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(Time.time - timeSinceOvercloak >= overcloakCooldown){
                Overcloak();
                timeSinceOvercloak = Time.time;
            } else {
                Debug.Log("Overcloak jest na cooldownie");
            }
        }

        UpdateAbilityDisplay();
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

        GameObject qteObject = Instantiate(qtePrefab, transform.position, Quaternion.identity);
        Destroy(qteObject, 0.25f);

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

    private void Shockwave()
    {
        float shockwaveRadius = 15f;
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, shockwaveRadius);

        foreach(Collider enemy in hitEnemies)
        {
            if(enemy.CompareTag("Enemy"))
            {
                levelManager.GetComponent<LevelManager>().AddPoints(enemy.gameObject.GetComponent<BasherController>().points);
                Destroy(enemy.gameObject);
            }
        }
    }

    private void Overcloak()
    {
        var weapon = weaponController.GetComponent<WeaponController>();
        originalRoF = weapon.rateOfFire;
        originalSpeed = this.moveSpeed;

        weapon.rateOfFire = weapon.rateOfFire / 2;
        this.moveSpeed = this.moveSpeed * 2;

        Invoke("RestoreOriginalValues", overcloakTime);
    }

    private void RestoreOriginalValues()
    {
        var weapon = weaponController.GetComponent<WeaponController>();
        weapon.rateOfFire = originalRoF;
        this.moveSpeed = originalSpeed;
    }

    private void ToggleImmune()
    {
        isImmune = !isImmune;
    }

    private void UpdateAbilityDisplay()
    {
        /*
        Dash: Ready ( Z )
        Shockwave: Ready ( X )
        Overcloak: Ready ( C )
        */
        string ready = "Ready";
        
        string dashDisplay = $@"Dash: {(Time.time - timeSinceDash >= dashCooldown ? ready : (dashCooldown - (Time.time - timeSinceDash)).ToString("0.00"))} ( Z )\n";
        string shockwaveDisplay = $@"Shockwave: {(Time.time - timeSinceShockwave >= shockwaveCooldown ? ready : (shockwaveCooldown - (Time.time - timeSinceShockwave)).ToString("0.00"))} ( X )\n";
        string overcloakDisplay = $@"Overcloak: {(Time.time - timeSinceOvercloak >= overcloakCooldown ? ready : (overcloakCooldown - (Time.time - timeSinceOvercloak)).ToString("0.00"))} ( C )\n";


        AbilityDisplay.GetComponent<TextMeshProUGUI>().text = dashDisplay + shockwaveDisplay + overcloakDisplay;
    }
}
