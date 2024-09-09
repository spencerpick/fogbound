using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro; 

public class IntroManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer; // Reference to the VideoPlayer
    [SerializeField] bool enableIntro = true; // Toggle intro to play or not play (dev purposes)
    [SerializeField] TextMeshProUGUI skipText; // Press enter to skip message

    [SerializeField] private List<MonoBehaviour> scriptsToDisable; // List of scripts to be disabled while intro plays

    void Start()
    {
        if (enableIntro)
        {
            DisableScripts(); // Disable all scripts in the list while the intro is playing
            videoPlayer.loopPointReached += OnVideoEnd;

            // Load the video from the StreamingAssets folder
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, "INTRO.mp4");
            videoPlayer.url = path;

            videoPlayer.Play(); // Play the intro video
            ShowSkipMessage(); // Display the enter to skip text
        }
        else
        {
            EnableScripts(); // If intro is disabled then enable all scripts straight away
        }
    }


    void Update()
    {
        // If enter key pressed during intro, then skip it
        if (Input.GetKeyDown(KeyCode.Return) && enableIntro)
        {
            SkipVideo();
        }
    }

    private void ShowSkipMessage()
    {
        if (skipText != null)
        {
            skipText.gameObject.SetActive(true); // Display the skip message
            skipText.text = "Press Enter to Skip"; 
        }
    }

    private void HideSkipMessage()
    {
        if (skipText != null)
        {
            skipText.gameObject.SetActive(false); // Hide the skip message
        }
    }

    private void DisableScripts() // Disables all scripts in the list (add them in the inspector guys)
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null && script.enabled)
            {
                script.enabled = false;
            }
        }
    }

    private void EnableScripts() // Re-enables all scripts once intro is done playing
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SkipVideo(); // Triggers the skip video to enable scripts and such once the video is done playing
    }

    private void SkipVideo()
    {
        EnableScripts(); 
        videoPlayer.Stop(); 
        HideSkipMessage(); 
    }
}
