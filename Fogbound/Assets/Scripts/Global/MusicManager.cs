using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // Stores music tracks
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from MusicManager.");
            return;
        }

        // Start playing the first track if there are tracks available
        if (musicTracks.Length > 0)
        {
            PlayNextTrack();
        }
    }

    void Update()
    {
        // Check if the current track has finished playing
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }

        if (Input.GetKeyDown(KeyCode.Asterisk) || Input.GetKeyDown(KeyCode.KeypadMultiply)) // For testing purposes
        {
            Debug.Log("Skipping to the next track...");
            PlayNextTrack();
        }
    
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return; 

        // Assign the next track
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();

        // Move to the next track, and loop back if needed
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }
}
