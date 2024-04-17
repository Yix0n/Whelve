using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float range = 15f;

    Transform player;

    public GameObject projectilePrefab;

    public GameObject projectileSpawnGO;

    Transform projectileSpawn;

    public float rateOfFire = 1; // strzał na sekunde

    float timeSinceLastFire = 0;

    public float projectileForce = 200;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        projectileSpawn = projectileSpawnGO.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFire += Time.deltaTime;
        Transform target = TagTargeter("Enemy");
        if (target != transform)
        {
            transform.LookAt(target.position + Vector3.up);
        
            if(timeSinceLastFire > rateOfFire)
            {
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);

                Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

                projectileRB.AddForce(projectileSpawn.transform.forward * projectileForce, ForceMode.VelocityChange);
                projectile.transform.LookAt(target.transform.position + Vector3.up);

                timeSinceLastFire = 0;

                Destroy(projectile, 5);
            }
        }
    }
    Transform TagTargeter(string tag)
    {

        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);


        Transform closestTarget = transform;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {

            Vector3 difference = target.transform.position - player.position;

            float distance = difference.magnitude;

            if (distance < closestDistance && distance < range)
            {
                closestTarget = target.transform;
                closestDistance = distance;
            }
        }
        return closestTarget;
    }

    Transform LegeacyTargeter()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        Transform target = transform;
        float targetDistance = Mathf.Infinity;

        foreach (Collider collider in collidersInRange)
        {
            GameObject model = collider.gameObject;

            if (model.transform.parent != null)
            {

                GameObject enemy = model.transform.parent.gameObject;

                if (enemy.CompareTag("Enemy"))
                {

                    Vector3 diference = player.position - enemy.transform.position;

                    float distance = diference.magnitude;
                    if (distance < targetDistance)
                    {

                        target = enemy.transform;
                        targetDistance = distance;
                    }
                }
            }


        }
        return target;
    }
}