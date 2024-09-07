using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
    public string loseLifeSoundPath = "Audios/LoseLife"; // Path to the lose life sound inside the Resources folder

    private int currentLives;
    private AudioSource audioSource; // Reference to AudioSource component

    // Reference to the parent canvas (where the hearts will be instantiated)
    private Transform canvasTransform;

    // Reference to the heart prefab
    private GameObject heartPrefab;

    // Starting position for the first heart
    public Vector2 initialPosition = new Vector2(-390, 220);

    // Distance between hearts
    public Vector2 heartOffset = new Vector2(70, 0);

    // List to keep track of heart instances
    private List<GameObject> hearts = new List<GameObject>();

    private AudioClip loseLifeSound; // Reference to the audio clip for losing life

    void Start()
    {
        // Find the Canvas in the scene
        canvasTransform = GameObject.Find("Canvas").transform;
        heartPrefab = Resources.Load<GameObject>("Prefabs/HeartPrefab"); // Adjust path if necessary
        // Initialize the player's lives to the maximum number of lives
        currentLives = maxLives;
        Debug.Log("Player starting with " + currentLives + " lives.");

        // Instantiate the heart prefabs and add them to the list
        for (int i = 0; i < currentLives; i++)
        {
            Vector2 heartPosition = initialPosition + i * heartOffset; // Calculate the position of each heart

            // Instantiate the heart as a child of the canvas, with the correct position in the canvas space
            GameObject heart = Instantiate(heartPrefab, canvasTransform);

            // Set the position in the UI space (anchored position)
            heart.GetComponent<RectTransform>().anchoredPosition = heartPosition;

            // Add the heart to the list
            hearts.Add(heart);
        }

        // Get the AudioSource component (add one if it doesn't exist)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Load the lose life sound from the Resources folder
        loseLifeSound = Resources.Load<AudioClip>(loseLifeSoundPath);

        // Log an error if the audio clip couldn't be loaded
        if (loseLifeSound == null)
        {
            Debug.LogError("Lose life sound could not be loaded from Resources folder at: " + loseLifeSoundPath);
        }
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;

            // Play the lose life sound if it's loaded
            if (loseLifeSound != null)
            {
                audioSource.PlayOneShot(loseLifeSound, 1.0f); 
            }

            // Destroy the last heart in the list (UI element)
            Destroy(hearts[currentLives]);
            hearts.RemoveAt(currentLives); // Remove the reference from the list

            // Print the remaining lives to the console
            Debug.Log("Player lost a life! Lives remaining: " + currentLives);

            // Check if player has no lives left
            if (currentLives <= 0)
            {
                GameOver();
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
