using UnityEngine;
using UnityEngine.InputSystem;

public class HeroControll : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 0.5f;
    public float autoAimRadius = 10f;
    private float nextShootTime = 0f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isDashing;
    private float dashEndTime;
    private Vector3 dashDirection;
    private float nextDashTime = 0f;
    public Transform respawnPoint;
    public float respawnTime = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        float currentYVelocity = rb.linearVelocity.y;

        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed + new Vector3(0f, currentYVelocity, 0f);

            if (Time.time >= dashEndTime)
            {
                isDashing = false;
            }
        }
        else
        {
            Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
            movement.y = currentYVelocity;
            rb.linearVelocity = movement;

            if (moveInput != Vector2.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0f, moveInput.y));
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 15f));
            }
        }
    }

    public void PerformDash()
    {
        if (!isDashing && moveInput != Vector2.zero && Time.time >= nextDashTime)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            nextDashTime = Time.time + dashCooldown;
            dashDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        }
    }
    public void PerformShoot()
    {
        if (Time.time >= nextShootTime && bulletPrefab != null && firePoint != null)
        {
            nextShootTime = Time.time + shootCooldown;
            Collider[] hits = Physics.OverlapSphere(transform.position, autoAimRadius);
            Transform closestEnemy = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("TeamEnemy"))
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestEnemy = hit.transform;
                    }
                }
            }
            Vector3 shootDirection = firePoint.forward;

            if (closestEnemy != null)
            {
                Vector3 enemyCenter = closestEnemy.position + Vector3.up * 0.5f;
                shootDirection = (enemyCenter - firePoint.position).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(shootDirection.x, 0, shootDirection.z));
            }
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
            Bullet bullet = bulletObj.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.Initialize("TeamEnemy");
                bullet.damage = 25f;
            }
        }
    }

    public void DieAndRespawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private System.Collections.IEnumerator RespawnRoutine()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
        }
        GetComponent<Collider>().enabled = false;
        foreach (Renderer r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        Canvas healthUI = GetComponentInChildren<Canvas>();
        if (healthUI != null) healthUI.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        if (respawnPoint != null) transform.position = respawnPoint.position;
        HealthManager hm = GetComponent<HealthManager>();
        if (hm != null) hm.Heal(hm.maxHealth);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
        }
        GetComponent<Collider>().enabled = true;
        foreach (Renderer r in GetComponentsInChildren<Renderer>()) r.enabled = true;
        if (healthUI != null) healthUI.enabled = true;
    }
}
