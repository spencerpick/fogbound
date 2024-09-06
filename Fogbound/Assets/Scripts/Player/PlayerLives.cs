using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
    public float heartSpacing = 10f; // Spacing between hearts

    private int currentLives;
    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        // Initialize the player's lives to the maximum number of lives
        currentLives = maxLives;
        Debug.Log("Player starting with " + currentLives + " lives.");
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;

            // Print the remaining lives to the console
            Debug.Log("Player lost a life! Lives remaining: " + currentLives);

            // Check if player has no lives left
            if (currentLives <= 0)
            {
                GameOver();
            }
        }
        else
        {
            Debug.Log("Player already has 0 lives.");
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Restart the level when the player runs out of lives
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
