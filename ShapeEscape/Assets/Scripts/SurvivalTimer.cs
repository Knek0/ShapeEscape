using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    // Time UI elements
    public TextMeshProUGUI time; 
    public TextMeshProUGUI best;

    // Timer variables
    private float timer = 0f;
    private float bestScore = 0f;
    private bool running = true;

    void Start()
    {
        // Load high score
        bestScore = PlayerPrefs.GetFloat("HighScore", 0f);
        best.text = "Best: " + bestScore.ToString("F2") + "s";
    }

    void Update()
    {
        // Stop timer on death
        if (PlayerDie.gameOver)
        {
            if (running)
            {
                running = false;
                CheckForNewHighScore();
            }
            return;
        }

        // Timer update
        timer += Time.deltaTime;
        time.text = "Time: " + timer.ToString("F2") + "s";
        if (timer > bestScore)
        {
            best.text = "Best: " + timer.ToString("F2") + "s";
        }
    }

    // Continuously checks for new high score
    void CheckForNewHighScore()
    {
        if (timer > bestScore)
        {
            bestScore = timer;
            PlayerPrefs.SetFloat("HighScore", bestScore);
            PlayerPrefs.Save();
        }
    }
}
