using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class Enemy : MonoBehaviour
{
    // Speed at which the enemy moves
    public float speed = 0;

    // reference to the player object's transform
    public Transform player;

    // rigidbody of the enemy
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        rb.MovePosition(pos);
    }
}