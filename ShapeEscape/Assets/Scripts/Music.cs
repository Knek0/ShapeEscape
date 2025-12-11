using UnityEngine;
public class Music : MonoBehaviour
{
    public AudioSource music;

    //Pitches of music for dead and alive states
    public float normalPitch = 1f;     
    public float deathPitch = 0.5f;
    public float fallSpeed = 1f;
    public float riseSpeed = 10f;

    // State tracking
    private bool dead = false;
    private static Music instance;

    // Ensures a persistent music instance on restart
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Smoothly change pitch based on player state
        float targetPitch = dead ? deathPitch : normalPitch;
        float speed = dead ? fallSpeed : riseSpeed;

        music.pitch = Mathf.Lerp(music.pitch, targetPitch, Time.deltaTime * speed);
    }

    public void OnPlayerDeath()
    {
        dead = true;
    }

    public void OnRestart()
    {
        dead = false;
    }
}