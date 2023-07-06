using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class PlayVideo : MonoBehaviour
{
    [Tooltip("Whether video should play on load")]
    public bool playAtStart = false;

    public VideoPlayer videoPlayer;

    private void Start()
    {
        if (playAtStart)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    public void Play()
    {
        videoPlayer.Play();
    }

    public void Stop()
    {
        videoPlayer.Stop();
    }

    public void TogglePlayStop()
    {
        bool isPlaying = !videoPlayer.isPlaying;
        SetPlay(isPlaying);
    }

    public void TogglePlayPause()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    public void SetPlay(bool value)
    {
        if (value)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

}