using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OutroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject Leaderboard;
    public GameObject Timer;

    public float timeLeft = 0;

    [SerializeField] private List<GameObject> scriptsToDisable; // Drag and drop scripts in the Inspector

    private void Start()
    {
        // Load the video from StreamingAssets folder
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "OUTRO.mp4");
        videoPlayer.url = videoPath;
    }

    public void playOutro()
    {
        foreach (GameObject script in scriptsToDisable)
        {
            if (script != null)
            {
                script.SetActive(false);
            }
        }

        videoPlayer.loopPointReached += showLeaderboard; // Play outro and show leaderboard when done
        videoPlayer.Play();
    }

    private void Update()
    {
        // If the player presses Enter during the outro, skip it
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SkipOutro();
        }
    }


    void showLeaderboard(VideoPlayer vp)
    {
        SkipOutro();
    }

    private void SkipOutro()
    {
        videoPlayer.Stop(); // Stop the video
        gameObject.SetActive(false); // Disable the Outro Manager

        // Enable leaderboard and set necessary properties
        Leaderboard leaderboard = Leaderboard.GetComponent<Leaderboard>();
        leaderboard.nameInputUI.SetActive(true);
        leaderboard.timeToSubmit = 600f - Timer.GetComponent<CountdownTimer>().timeRemaining;

        Timer.GetComponent<CountdownTimer>().stopTimer(); // Stop the timer
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor for input
    }
}
