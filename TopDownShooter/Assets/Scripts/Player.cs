using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    // Speed at which the player moves
    public float speed = 0;

    // movement along X and Y axes
    private float movementX;
    private float movementY;

    // mouse position
    private Vector2 mouseScreenPos;

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

        mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorld.z = transform.position.z;
        Vector2 dir = (mouseWorld - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f; // Adjusting angle to point the top of the sprite towards the mouse
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
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

    public void OnAttack(InputValue attackValue)
    {
        Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation);
    }
}