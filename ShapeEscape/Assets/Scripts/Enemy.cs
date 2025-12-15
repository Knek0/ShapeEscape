using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class Enemy : MonoBehaviour
{
    // Reference to PlayerDie script to check game over state
    public PlayerDie playerDieScript;

    // Speed at which the enemy moves
    public float speed = 3;

    // player transform
    private Transform player;

    // rigidbody of the enemy
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get player transform on spawn
        rb = GetComponent<Rigidbody2D>();
        GameObject playerPostion = GameObject.FindGameObjectWithTag("Player");
        player = playerPostion.transform;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        // Move towards the player
        Vector3 pos = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        rb.MovePosition(pos);
    }
}