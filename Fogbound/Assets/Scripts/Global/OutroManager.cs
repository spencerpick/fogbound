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

        videoPlayer.loopPointReached += showLeaderboard;
        videoPlayer.Play();
    }

    void showLeaderboard(VideoPlayer vp)
    {
        gameObject.SetActive(false);
        Leaderboard leaderboard = Leaderboard.GetComponent<Leaderboard>(); // Get the leaderboard script

        leaderboard.nameInputUI.SetActive(true);
        leaderboard.timeToSubmit = 600f - Timer.GetComponent<CountdownTimer>().timeRemaining;

        Timer.GetComponent<CountdownTimer>().stopTimer();
        Cursor.lockState = CursorLockMode.None;
    }
}
