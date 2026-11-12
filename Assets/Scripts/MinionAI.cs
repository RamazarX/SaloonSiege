using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public Transform targetBase;
    public float attackDamage = 10f;
    public float attackRange = 8f;
    public float aggroRadius = 15f;
    public float attackCooldown = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private NavMeshAgent agent;
    private float nextAttackTime = 0f;
    private Transform currentTarget;
    private string enemyTag;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (gameObject.CompareTag("TeamAlly"))
            enemyTag = "TeamEnemy";
        else if (gameObject.CompareTag("TeamEnemy"))
            enemyTag = "TeamAlly";

        if (targetBase != null)
            agent.SetDestination(targetBase.position);
    }

    void Update()
    {
        FindClosestEnemy();
        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);
            if (distance <= attackRange)
            {
                agent.isStopped = true;
                agent.ResetPath();
                Vector3 lookDir = currentTarget.position - transform.position;
                lookDir.y = 0;
                if (lookDir != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(lookDir);
                }
                if (Time.time >= nextAttackTime)
                {
                    AttackTarget(currentTarget.gameObject);
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(currentTarget.position);
            }
        }
    }

    void FindClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroRadius);

        Transform bestTarget = null;
        int highestPriority = -1;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag(enemyTag))
            {
                int priority = 0;
                if (hit.GetComponent<MinionAI>() != null)
                    priority = 4;
                else if (hit.GetComponent<HealthManager>() != null && hit.GetComponent<HealthManager>().isBase)
                    priority = 3;
                else if (hit.GetComponent<CapturePoint>() != null)
                    priority = 2;
                else if (hit.GetComponent<HeroControll>() != null)
                    priority = 1;

                float distance = Vector3.Distance(transform.position, hit.transform.position);

                if (priority > highestPriority || (priority == highestPriority && distance < closestDistance))
                {
                    highestPriority = priority;
                    closestDistance = distance;
                    bestTarget = hit.transform;
                }
            }
        }

        currentTarget = bestTarget;
        if (currentTarget == null && targetBase != null)
        {
            currentTarget = targetBase;
        }
    }

    void AttackTarget(GameObject target)
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 aimDirection = (target.transform.position + Vector3.up * 0.5f) - firePoint.position;
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(aimDirection));
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Initialize(enemyTag);
                bullet.damage = attackDamage;
            }
        }
    }
}