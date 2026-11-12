using UnityEngine;

public class HealZone : MonoBehaviour
{
    public string targetTag = "TeamAlly";
    public float healRadius = 5f; 
    public float healAmount = 10f;
    public float healInterval = 1f;      

    private float nextHealTime;

    void Update()
    {
        if (Time.time >= nextHealTime)
        {
            HealAlliesInZone();
            nextHealTime = Time.time + healInterval;
        }
    }

    void HealAlliesInZone()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, healRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                HealthManager hm = hit.GetComponent<HealthManager>();
                if (hm != null)
                {
                    hm.Heal(healAmount);
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, healRadius);
    }
}