using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // Array to hold your music tracks
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
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return; // Make sure there are tracks available

        // Assign the next track
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();

        // Move to the next track, and loop back if needed
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }
}
