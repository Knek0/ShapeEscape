using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Audiosource
    private AudioSource audioSource;

    // Speed at which the player moves
    public float speed = 40;

    // Speed at which the player rotates
    public float rotationSpeed = 5;

    // Dashing
    public AudioClip dashSound;
    public float dashTime = 0.3f;
    public float dashCooldown = 5f;
    public float dashSpeed = 25f;
    private bool canDash = true;
    private bool isDashing = false;
    public ParticleSystem dashReady;
    public TrailRenderer dashTrail;

    // Movement along X and Y axes
    private float movementX;
    private float movementY;
    Vector2 movement;

    // Layers
    private int playerLayer;
    private int enemyLayer;

    // Input direction
    public Vector3 inputDirection { get; private set; }

    // Rigidbody of the player
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        dashTrail.emitting = false;
        dashReady.Play();
    }

    // Gets audioSource on Awake
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // FixedUpdate is called once per frame
    private void FixedUpdate()
    {
        if (isDashing) return;

        else
        {
            // Stop dashReady if player is dead
            if (rb.simulated == false)
            {
                dashReady.Stop();
            }

            // Movement vector using the X and Y inputs.
            movement = new(movementX, movementY);

            // Apply force to the Rigidbody to move the player.
            rb.AddForce(movement * speed);

            // Rotation
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

    public void OnJump(InputValue value)
    {
        // Dash on space pressed
        if (canDash)
        {
            StartCoroutine(Dash());
            dashReady.Stop();
        }
    }

    private IEnumerator Dash()
    {
        // Dashing
        canDash = false;
        isDashing = true;

        // Direction of dash
        Vector2 dashDirection = new Vector2(movementX, movementY).normalized;

        // Dash in direction of rotation if no movement input detected
        if (dashDirection == Vector2.zero)
        {
            dashDirection = transform.up.normalized;
        }

        // Ignore collisions with enemies
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        // Enable Dash trail
        dashTrail.Clear();
        dashTrail.emitting = true;

        // Apply force
        rb.linearVelocity = dashDirection * dashSpeed;
        isDashing = false;

        // Play dash sound and dash particle effect
        audioSource.PlayOneShot(dashSound);

        // Dash time
        yield return new WaitForSeconds(dashTime);

        // Disable dash trail
        dashTrail.emitting = false;
        isDashing = false;

        // Collision with Enemies back on
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        // Dash cooldown
        yield return new WaitForSeconds(dashCooldown);
        dashReady.Play();
        canDash = true;
    }
}