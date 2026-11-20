using UnityEngine;
using UnityEngine.Video;

public class LoopVids : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoFileName;

    void Start()
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(
            Application.streamingAssetsPath, videoFileName);

        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }
}