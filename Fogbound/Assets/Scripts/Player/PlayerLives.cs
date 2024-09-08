using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For TextMeshPro

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
    private int currentLives;

    public GameObject heartContainer;
    public GameObject heartPrefab;
    public float heartOffset = 110f; // Horizontal offset between hearts

    private List<GameObject> heartImages = new List<GameObject>();

    // Reference to the TextMeshProUGUI component for the lives display
    public TextMeshProUGUI livesText;

    void Start()
    {
        // Initialize the player's lives to the maximum number of lives
        currentLives = maxLives;
        Debug.Log("Player starting with " + currentLives + " lives.");

        CreateHearts();
    }

    private void CreateHearts()
    {
        for (int i = 0; i < maxLives; i++)
        {
            // Instantiate the heart prefab and set it as a child of the heart container
            GameObject heart = Instantiate(heartPrefab, heartContainer.transform);

            RectTransform heartTransform = heart.GetComponent<RectTransform>();
            heartTransform.anchoredPosition = new Vector2(i * heartOffset, 0);

            // Add the heart to the list
            heartImages.Add(heart);
        }
    }

    private void RemoveHeart()
    {
        if (heartImages.Count > 0)
        {
            // Find the last heart in the list
            GameObject heartToRemove = heartImages[heartImages.Count - 1];

            // Remove the heart from the container and destroy the object
            heartImages.Remove(heartToRemove);
            Destroy(heartToRemove);
        }
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;

            // Print the remaining lives to the console
            Debug.Log("Player lost a life! Lives remaining: " + currentLives);

            // Update the lives text on the UI
            RemoveHeart();

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
    /*private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives; // Display the current lives
        }
    }*/

    private void UpdateLivesDisplay()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentLives)
            {
                // Enable the heart image if the player has that life
                heartImages[i].SetActive(true);
            }
            else
            {
                // Disable the heart image if the player has lost that life
                heartImages[i].SetActive(false);
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Restart the level when the player runs out of lives
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
