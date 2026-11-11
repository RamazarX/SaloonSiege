using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public Transform targetBase;
    public float attackDamage = 10f;
    public float attackRange = 2.5f;
    public float aggroRadius = 5f;
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

        if (currentTarget == null)
        {
            FindClosestEnemy();
        }


        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            if (distanceToTarget <= attackRange)
            {

                agent.isStopped = true;

                Vector3 direction = (currentTarget.position - transform.position).normalized;
                direction.y = 0; 
                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);

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
        else
        {
            if (targetBase != null)
            {
                agent.isStopped = false;
                agent.SetDestination(targetBase.position);
            }
        }
    }

    void FindClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroRadius);
        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag(enemyTag))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTarget = hit.transform;
                }
            }
        }

        currentTarget = bestTarget;
        if (currentTarget == null && targetBase != null)
        {
            if (Vector3.Distance(transform.position, targetBase.position) <= attackRange)
            {
                currentTarget = targetBase;
            }
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