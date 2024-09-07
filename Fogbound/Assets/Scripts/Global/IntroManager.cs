using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer; // Reference to the VideoPlayer
    [SerializeField] bool enableIntro = true; // Toggle for enabling or disabling the intro video

    // Manually add all scripts you want to disable until the video ends
    [SerializeField] private List<MonoBehaviour> scriptsToDisable; // Drag and drop scripts in the Inspector

    void Start()
    {
        if (enableIntro)
        {
            DisableScripts(); // Disable all scripts at the start
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Play(); // Play the video
        }
        else
        {
            EnableScripts(); // If intro is disabled, enable all scripts immediately
        }
    }

    // Method to disable all specified scripts
    private void DisableScripts()
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null && script.enabled)
            {
                script.enabled = false;
            }
        }
    }

    // Method to enable all specified scripts
    private void EnableScripts()
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }
    }

    // Called when the video ends
    void OnVideoEnd(VideoPlayer vp)
    {
        EnableScripts(); // Enable all scripts when the video ends
        videoPlayer.Stop(); // Stop the video
    }
}
