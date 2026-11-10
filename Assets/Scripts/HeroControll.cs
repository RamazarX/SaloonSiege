using UnityEngine;
using UnityEngine.InputSystem;

public class HeroControll : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float dashForce = 20f;

    private Rigidbody rb;
    private Vector2 moveInput;

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
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        movement.y = rb.linearVelocity.y;
        rb.linearVelocity = movement;

        if (moveInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0f, moveInput.y));
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 15f));
        }
    }
    public void PerformDash()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 dashDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            rb.AddForce(dashDir * dashForce, ForceMode.Impulse);
        }
    }
}
