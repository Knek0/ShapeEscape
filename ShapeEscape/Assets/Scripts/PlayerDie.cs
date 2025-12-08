using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerDie : MonoBehaviour
{
    public GameObject gameOverText;

    public static bool gameOver = false;

    public InputAction anyKey;

    private void OnEnable()
    {
        anyKey.Enable();
    }

    private void OnDisable()
    {
        anyKey.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameOverText.SetActive(false);
    }
    private void Awake()
    {
        gameOver = false;
    }

    private void Die()
    {
        // Prevents multiple death triggers
        if (gameOver) return;
        gameOver = true;

        // Show game over text
        gameOverText.SetActive(true);

        // Disable player movement and physics
        GetComponent<Rigidbody2D>().simulated = false;

        // Hide sprite
        GetComponent<SpriteRenderer>().enabled = false;

        // Listen for restart
        anyKey.performed += Restart;
    }

    private void Restart(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Die on collison with enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Die();
        }
    }

}
