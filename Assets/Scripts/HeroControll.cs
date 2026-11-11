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
    private float nextShootTime = 0f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isDashing;
    private float dashEndTime;
    private Vector3 dashDirection;
    private float nextDashTime = 0f;

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
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletObj.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.Initialize("TeamEnemy");
                bullet.damage = 25f;
            }
        }
    }
}
