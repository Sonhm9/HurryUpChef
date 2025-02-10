using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private float moveSpeed = 300f;
    private float turnSpeed = 15f;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;

    private InputAction moveAction;

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    public void HandleMovement()
    {
        // 이동 방향 입력
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        movement = new Vector3(-moveZ, 0, moveX);

        if (moveX == 0f && moveZ == 0f)
        {
            StopMoving();
        }

        animator.SetBool("Walk", movement != Vector3.zero);
    }

    public void ApplyMovement()
    {
        if (movement == Vector3.zero)
        {
            return;
        }

        // 이동
        rb.linearVelocity = movement * moveSpeed * Time.fixedDeltaTime;
        // 이동시 몸체 회전
        Quaternion direction = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, turnSpeed * Time.fixedDeltaTime);
    }

    public void StopMoving()
    {
        rb.linearVelocity = Vector3.zero;
    }
}