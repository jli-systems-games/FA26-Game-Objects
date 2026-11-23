using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    public GameObject mainCameraRef;
    public GameObject switchCamera;
    public GameObject currentCamera;

    public VideoPlayer videoPlayer;
    public string videoFileName;
    public PlayerController playerController;
    public SwapMusic swapMusic;
    public HintUI hintUI;
    public TodoList todoList;

    public KeyCode interactKey = KeyCode.E;
    public KeyCode exitKey = KeyCode.Q;

    private bool playerTrigger;
    private bool counted;
    private float watchedSeconds;

    void Start()
    {
        currentCamera = mainCameraRef;
        switchCamera.SetActive(false);

        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(
            Application.streamingAssetsPath, videoFileName);
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (playerTrigger &&
            currentCamera == mainCameraRef &&
            playerController.enabled &&
            mainCameraRef.activeInHierarchy &&
            Input.GetKeyDown(interactKey))
        {
            WatchVideo();
        }

        if (currentCamera == switchCamera)
        {
            if (!counted)
            {
                watchedSeconds += Time.deltaTime;

                if (watchedSeconds >= 30f && todoList != null)
                    counted = todoList.MarkWatched(videoPlayer);
            }

            if (Input.GetKeyDown(exitKey) || !playerTrigger)
                ExitVideo();
        }
    }

    void WatchVideo()
    {
        if (hintUI != null)
            hintUI.BeginInteraction(this);

        playerController.enabled = false;
        currentCamera.SetActive(false);
        switchCamera.SetActive(true);
        currentCamera = switchCamera;

        if (swapMusic != null)
            swapMusic.PauseMusic();

        videoPlayer.Play();
    }

    void ExitVideo()
    {
        videoPlayer.Pause();

        currentCamera.SetActive(false);
        mainCameraRef.SetActive(true);
        currentCamera = mainCameraRef;
        playerController.enabled = true;

        if (swapMusic != null)
            swapMusic.ResumeMusic();

        if (hintUI != null)
            hintUI.EndInteraction(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = true;

            if (hintUI != null)
                hintUI.EnterRange(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;

            if (hintUI != null)
                hintUI.LeaveRange(this);

            if (currentCamera == switchCamera)
                ExitVideo();
        }
    }
}