using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 600f; // 10 minute timer till game over
    public TextMeshProUGUI timerText;  // UI showing current time left
    private bool timerIsRunning = false;  // Whether the timer is running or not
    private PlayerLives playerLives; // Reference to player lives so can cause player to die if timer reaches 0

    void Start()
    {
        playerLives = FindObjectOfType<PlayerLives>(); // Find reference to player lives object

        timerIsRunning = true; // Set the timer to run
    }

    void Update()
    {
        if (timerIsRunning) // Countdown the timer
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

    }

    public void stopTimer() 
    {
        timerIsRunning = false;
    }

    void DisplayTime(float timeToDisplay) // Display the time left in minutes and seconds
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    
    void OnTimerEnd() // Called when the timer reaches zero
    {
        playerLives.GameOver();
    }


}
