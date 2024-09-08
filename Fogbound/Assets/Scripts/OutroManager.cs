using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OutroManager : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public GameObject Leaderboard;
    public GameObject Timer;

    public void playOutro()
    {
        videoPlayer.loopPointReached += showLeaderboard;
        videoPlayer.Play();
    }

    void showLeaderboard(VideoPlayer vp)
    {
        videoPlayer.Stop(); // Stop the video once it finishes
        Leaderboard leaderboard = Leaderboard.GetComponent<Leaderboard>(); // Get the leaderboard script
        CountdownTimer timer = Timer.GetComponent<CountdownTimer>(); // Get the countdown timer script

        // Add the remaining time to the leaderboard
        if (timer != null)
        {
            leaderboard.AddScore(timer.timeRemaining);
        }

        // Activate the leaderboard UI (assuming it's inactive until the video finishes)
        leaderboard.backgroundImage.SetActive(true);
    }
}
