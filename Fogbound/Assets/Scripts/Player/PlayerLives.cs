using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For TextMeshPro

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
    private int currentLives;

    // Reference to the TextMeshProUGUI component for the lives display
    public TextMeshProUGUI livesText;

    void Start()
    {
        // Initialize the player's lives to the maximum number of lives
        currentLives = maxLives;
        Debug.Log("Player starting with " + currentLives + " lives.");

        // Update the lives text on the UI at the start
        UpdateLivesText();
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;

            // Print the remaining lives to the console
            Debug.Log("Player lost a life! Lives remaining: " + currentLives);

            // Update the lives text on the UI
            UpdateLivesText();

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

    // Method to update the TextMeshPro UI with the current number of lives
    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives; // Display the current lives
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Restart the level when the player runs out of lives
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
