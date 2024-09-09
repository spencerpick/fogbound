using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public class Leaderboard : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText; // Text component to display the leaderboard
    public GameObject backgroundImage; // The background image for the leaderboard
    public TMP_InputField nameInputField; // Input field for the player's name
    public GameObject nameInputUI; // UI panel with the name input and submit button

    public float timeToSubmit = 0f;
    private string leaderboardFilePath; // File path for saving and loading

    private List<PlayerScore> playerScores = new List<PlayerScore>(); // List to store player names and scores

    private struct PlayerScore
    {
        public string playerName;
        public float score;

        public PlayerScore(string name, float score)
        {
            this.playerName = name;
            this.score = score;
        }
    }

    void Start()
    {
        backgroundImage.SetActive(false);
        leaderboardFilePath = Application.persistentDataPath + "/leaderboard.txt"; // Set the path for saving/loading the leaderboard

        LoadLeaderboard(); // Load leaderboard at the start
    }

    // Method to handle submitting the player's name and time
    public void SubmitNameAndScore()
    {
        string playerName = nameInputField.text; // Get the player's name from the input field

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "UndefinedPlayer";
        }

        AddScore(playerName, timeToSubmit);
        nameInputUI.SetActive(false); // Hide the name input UI after submission
        backgroundImage.SetActive(true); // Show the leaderboard background
        UpdateLeaderboardDisplay();
        SaveLeaderboard(); // Save the updated leaderboard
    }

    // Method to add a score to the leaderboard with player's name
    private void AddScore(string playerName, float timeRemaining)
    {
        PlayerScore newScore = new PlayerScore(playerName, timeRemaining);
        playerScores.Add(newScore); // Add the player name and score to the list

        // Sort the scores in ascending order (best times first)
        playerScores.Sort((a, b) => a.score.CompareTo(b.score));

        // Update the leaderboard UI text
        UpdateLeaderboardDisplay();
    }

    // Method to update the leaderboard UI
    private void UpdateLeaderboardDisplay()
    {
        leaderboardText.text = "Leaderboard\n"; // Clear and reset the leaderboard text

        // Display each player name and score
        for (int i = 0; i < playerScores.Count; i++)
        {
            leaderboardText.text += (i + 1) + ". " + playerScores[i].playerName + ": " + playerScores[i].score.ToString("F2") + " seconds\n";
        }
    }

    // Method to save the leaderboard to a file
    private void SaveLeaderboard()
    {
        using (StreamWriter writer = new StreamWriter(leaderboardFilePath))
        {
            foreach (PlayerScore playerScore in playerScores)
            {
                writer.WriteLine(playerScore.playerName + "," + playerScore.score.ToString());
            }
        }
    }

    // Method to load the leaderboard from a file
    private void LoadLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            using (StreamReader reader = new StreamReader(leaderboardFilePath))
            {
                playerScores.Clear(); // Clear current scores before loading

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splitLine = line.Split(',');
                    if (splitLine.Length == 2)
                    {
                        string playerName = splitLine[0];
                        float score = float.Parse(splitLine[1]);
                        playerScores.Add(new PlayerScore(playerName, score));
                    }
                }
            }
        }
    }
}
