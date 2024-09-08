using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Assuming you are using TextMeshPro for UI text

public class Leaderboard : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText; // Text component to display the leaderboard
    public GameObject backgroundImage;
    private List<float> scores = new List<float>(); // List to store the leaderboard scores

    void Start()
    {
        backgroundImage.SetActive(false);
        UpdateLeaderboardDisplay();
    }

    // Method to add a score to the leaderboard
    public void AddScore(float timeRemaining)
    {
        scores.Add(timeRemaining); // Add the score to the list

        // Sort the scores in descending order (best times first)
        scores.Sort((a, b) => b.CompareTo(a));

        // Update the leaderboard UI text
        UpdateLeaderboardDisplay();
    }

    // Method to update the leaderboard UI
    private void UpdateLeaderboardDisplay()
    {
        leaderboardText.text = "Leaderboard:\n"; // Clear and reset the leaderboard text

        // Display each score
        for (int i = 0; i < scores.Count; i++)
        {
            leaderboardText.text += (i + 1) + ". " + scores[i].ToString("F2") + " seconds\n"; // Display with two decimal places
        }
    }
}
