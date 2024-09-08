using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For UI elements

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
    private int currentLives;

    public GameObject heartContainer;
    public GameObject heartPrefab;
    public float heartOffset = 110f; // Horizontal offset between hearts

    private List<GameObject> heartImages = new List<GameObject>();

    public AudioClip loseLifeSound;
    private AudioSource audioSource;

    public Image damageFlashImage; // Reference to the UI Image for damage feedback
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f); // Red color with transparency
    public float flashDuration = 0.5f; // How long the flash lasts

    // Reference to CameraShake script
    public CameraShake cameraShake;

    // Reference to Game Over panel and Try Again button
    public GameObject gameOverPanel; // Reference to Game Over panel
    public Button tryAgainButton;    // Reference to Try Again button

    void Start()
    {
        // Initialize the player's lives to the maximum number of lives
        currentLives = maxLives;
        Debug.Log("Player starting with " + currentLives + " lives.");

        // Initialize AudioSource component (add one if it doesn't exist)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        CreateHearts();

        // Ensure the damage flash image starts invisible
        if (damageFlashImage != null)
        {
            damageFlashImage.color = Color.clear;
        }

        // Get the CameraShake component from the main camera
        if (cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }

        // Disable the Game Over panel at the start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Add listener to Try Again button
        if (tryAgainButton != null)
        {
            tryAgainButton.onClick.AddListener(RestartGame);
        }
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

            // Play the lose life sound
            if (loseLifeSound != null)
            {
                Debug.Log("Lose life sound is assigned.");
                audioSource.PlayOneShot(loseLifeSound);
            }
            else
            {
                Debug.LogWarning("Lose life sound not assigned.");
            }

            // Trigger visual feedback (screen flash)
            StartCoroutine(FlashDamageEffect());

            // Trigger camera shake
            if (cameraShake != null)
            {
                cameraShake.TriggerShake();
            }
            else
            {
                Debug.LogWarning("CameraShake script is not assigned.");
            }
        }
    }

    // Coroutine for the screen flash effect
    private IEnumerator FlashDamageEffect()
    {
        if (damageFlashImage != null)
        {
            // Set the image color to the damage color
            damageFlashImage.color = damageColor;

            // Wait for the duration of the flash
            yield return new WaitForSeconds(flashDuration);

            // Gradually fade out the flash by setting it back to transparent
            damageFlashImage.color = Color.clear;
        }
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;

            // Print the remaining lives to the console
            Debug.Log("Player lost a life! Lives remaining: " + currentLives);

            // Update the lives UI
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

    private void GameOver()
    {
        Debug.Log("Game Over!");

        // Show the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Pause the game by setting timeScale to 0
        Time.timeScale = 0f;

        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Method for restarting the game
    public void RestartGame()
    {
        // Reset the time scale to 1 (unpause the game)
        Time.timeScale = 1f;

        // Lock the cursor again and hide it if necessary
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
