using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifeTime = 2f;

    private string targetTag;
    public void Initialize(string enemyTag)
    {
        targetTag = enemyTag;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Летим вперед по локальной оси Z
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            HealthManager hm = other.GetComponent<HealthManager>();
            if (hm != null)
            {
                hm.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}