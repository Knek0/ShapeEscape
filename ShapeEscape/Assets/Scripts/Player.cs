using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{ 
    // Speed at which the player moves
    public float speed = 15;

    // Speed at which the player rotates
    public float rotationSpeed = 5;

    // movement along X and Y axes
    private float movementX;
    private float movementY;

    //input direction
    public Vector3 inputDirection { get; private set; }

    // rigidbody of the player
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called once per frame
    private void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector2 movement = new(movementX, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);

        //rotation
        Vector2 dir = new Vector2(movementX, movementY);

        // Dead zone
        if (dir.sqrMagnitude < 0.01f)
            return;

        // Don't rotate if no movement
        if (movement == Vector2.zero)
            return;

        // Calculate the angle and rotate towards the movement direction.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
        inputDirection = new Vector2(movementX, movementY);
    }
}