using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required to reload the scene
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 600f; // 10 minutes
    public TextMeshProUGUI timerText;  // UI Text component to show the time
    private bool timerIsRunning = false;

    void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                OnTimerEnd(); // Time's up!
            }
        }

        // For testing purposes, press the 'R' key to manually restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame(); // Restart the game when 'R' key is pressed
        }
    }

    // Time
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Called when the timer reaches zero
    void OnTimerEnd()
    {
        Debug.Log("Time's up! Game Over.");
        EndGame();
    }

    // Placeholder for game-over logic
    void EndGame()
    {
        Debug.Log("Ending the game.");
        // Example: you can display a "Game Over" screen or trigger any other logic
        RestartGame(); // Automatically restart the game after time's up
    }

    // Restarts the game by reloading the current scene
    public void RestartGame()
    {
        Debug.Log("Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
