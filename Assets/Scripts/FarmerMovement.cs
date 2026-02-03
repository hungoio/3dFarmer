using UnityEngine;
using UnityEngine.InputSystem;

public class FarmerMovement : MonoBehaviour
{
    public float speed = 4f;
    public float rotateSpeed = 10f;
    public float gravity = -20f;

    CharacterController controller;
    Animator animator;
    Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Vector2 input = ReadKeyboard();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        bool isMoving = move.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            Rotate(move);
        }

        // BÁM TERRAIN
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -5f;

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove =
            move * speed + Vector3.up * velocity.y;

        controller.Move(finalMove * Time.deltaTime);
    }

    Vector2 ReadKeyboard()
    {
        if (Keyboard.current == null) return Vector2.zero;

        return new Vector2(
            (Keyboard.current.dKey.isPressed ? 1 : 0) -
            (Keyboard.current.aKey.isPressed ? 1 : 0),
            (Keyboard.current.wKey.isPressed ? 1 : 0) -
            (Keyboard.current.sKey.isPressed ? 1 : 0)
        );
    }

    void Rotate(Vector3 move)
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(move),
            rotateSpeed * Time.deltaTime
        );
    }
}
