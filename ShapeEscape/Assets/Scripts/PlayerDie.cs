using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerDie : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject deathParticles;

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

    private IEnumerator DieCoroutine()
    {
        // Prevents multiple death triggers
        if (gameOver) yield break;
        gameOver = true;

        // Spawn death particles
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();

        // Disable player movement and physics
        GetComponent<Rigidbody2D>().simulated = false;

        // Hide sprite
        GetComponent<SpriteRenderer>().enabled = false;

        // wait before showing game over UI
        yield return new WaitForSecondsRealtime(1.2f);

        // Show game over text
        gameOverText.SetActive(true);

        // Listen for restart
        anyKey.performed += Restart;
    }

    private void Die()
    {
        StartCoroutine(DieCoroutine());
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
