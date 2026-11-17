using UnityEngine;
using UnityEngine.Video;

public class CameraSwitch : MonoBehaviour
{
    public GameObject mainCameraRef;
    public GameObject switchCamera;
    public GameObject currentCamera;

    public GameObject interactionText;
    public VideoPlayer videoPlayer;
    public string videoFileName;

    public PlayerController playerController;

    public KeyCode interactKey = KeyCode.E;
    public KeyCode exitKey = KeyCode.Q;

    private bool playerTrigger;

    void Start()
    {
        currentCamera = mainCameraRef;
        switchCamera.SetActive(false);
        interactionText.SetActive(false);

        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(
            Application.streamingAssetsPath, videoFileName);
    }

    void Update()
    {
        if (playerTrigger &&
            currentCamera == mainCameraRef &&
            mainCameraRef.activeSelf &&
            Input.GetKeyDown(interactKey))
        {
            WatchVideo();
        }

        if (currentCamera == switchCamera &&
            (Input.GetKeyDown(exitKey) || !playerTrigger))
        {
            ExitVideo();
        }
    }

    void WatchVideo()
    {
        interactionText.SetActive(false);
        playerController.enabled = false;

        currentCamera.SetActive(false);
        switchCamera.SetActive(true);
        currentCamera = switchCamera;

        videoPlayer.Play();
    }

    void ExitVideo()
    {
        videoPlayer.Pause();

        currentCamera.SetActive(false);
        mainCameraRef.SetActive(true);
        currentCamera = mainCameraRef;

        playerController.enabled = true;
        interactionText.SetActive(playerTrigger);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = true;
            interactionText.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;
            interactionText.SetActive(false);
        }
    }
}