using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{

    [SerializeField] VideoPlayer videoPlayer; 
    [SerializeField] GameObject player;
    [SerializeField] bool enableIntro;

    // Start is called before the first frame update
    void Start()
    {
        if(enableIntro == true)
        {
            videoPlayer.loopPointReached +=  OnVideoEnd;
            videoPlayer.Play();

            // Disable the player movement and other components here
            if (player != null)
            {
                player.GetComponent<PlayerMovement>().enabled = false;
            }

        }
        else
        {
            return;
        }
    }

    
    void OnVideoEnd(VideoPlayer vp)
    {
        if(player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            videoPlayer.Stop();
        }
    }
}
