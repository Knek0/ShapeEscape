using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Speed at which the player moves
    public float speed = 0;

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
    private void Update()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector2 movement = new(movementX, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
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