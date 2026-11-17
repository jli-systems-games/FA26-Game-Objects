using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System.Collections;

public class HidisSpeak : MonoBehaviour
{
    public Animator npcAnimator;
    public TMP_Text dialogueText;
    public GameObject HiCanvas;

    public GameObject mainCameraRef;
    public GameObject hiCamera;
    public PlayerController playerController;

    public VideoPlayer ohNoVideo;

    public string[] words;
    public string[] waitWords;

    public float typingSpeed = 0.05f;
    public float[] waitTimes = { 20f, 40f, 60f, 120f };

    public KeyCode continueKey = KeyCode.E;
    public KeyCode exitKey = KeyCode.Q;

    private bool storyFinished;
    private bool isTalking;
    private bool videoFinished;

    void Start()
    {
        dialogueText.text = "";
        hiCamera.SetActive(false);
        HiCanvas.SetActive(false);

        ohNoVideo.loopPointReached += VideoFinished;
    }

    void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(exitKey))
            {
                StopAllCoroutines();
                storyFinished = true;
                ConEnd();
            }
        }
        else if (videoFinished &&
                 !storyFinished &&
                 mainCameraRef.activeInHierarchy &&
                 playerController.enabled)
        {
            StartCoroutine(HiSpeaking());
        }
    }

    void VideoFinished(VideoPlayer vp)
    {
        videoFinished = true;
    }

    IEnumerator HiSpeaking()
    {
        SwitchCamera();

        foreach (string line in words)
        {
            yield return TypeText(line, "HiSpeak");

            float startTime = Time.time;
            int waitIndex = 0;

            Coroutine waitDialogue = null;

            yield return null;

            while (!ContinuePressed())
            {
                float timer = Time.time - startTime;

                if (waitIndex < waitTimes.Length &&
                    timer >= waitTimes[waitIndex])
                {
                    if (waitDialogue != null)
                    {
                        StopCoroutine(waitDialogue);
                    }

                    string animationName = "Wait" + (waitIndex + 1);

                    if (waitIndex < waitWords.Length)
                    {
                        waitDialogue = StartCoroutine(
                            TypeText(waitWords[waitIndex], animationName));
                    }
                    else
                    {
                        npcAnimator.Play(animationName, 0, 0f);
                    }

                    waitIndex++;
                }

                yield return null;
            }

            if (waitDialogue != null)
            {
                StopCoroutine(waitDialogue);
            }
        }

        storyFinished = true;
        ConEnd();
    }

    void SwitchCamera()
    {
        isTalking = true;

        playerController.enabled = false;

        mainCameraRef.SetActive(false);
        hiCamera.SetActive(true);
        HiCanvas.SetActive(true);
    }

    void ConEnd()
    {
        dialogueText.text = "";
        HiCanvas.SetActive(false);

        npcAnimator.Play("HiIdle", 0, 0f);

        hiCamera.SetActive(false);
        mainCameraRef.SetActive(true);

        playerController.enabled = true;
        isTalking = false;
    }

    IEnumerator TypeText(string fullText, string animationName)
    {
        dialogueText.text = "";

        npcAnimator.Play(animationName, 0, 0f);

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    bool ContinuePressed()
    {
        return Input.GetMouseButtonDown(0) ||
               Input.GetKeyDown(continueKey);
    }

    void OnDestroy()
    {
        if (ohNoVideo != null)
        {
            ohNoVideo.loopPointReached -= VideoFinished;
        }
    }
}