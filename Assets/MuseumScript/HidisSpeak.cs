using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System.Collections;

public class HidisSpeak : MonoBehaviour
{
    public Animator HidisAnim;
    public TMP_Text HidisText;
    public GameObject HiCanvas;

    public GameObject mainCameraRef;
    public GameObject hiCamera;
    public PlayerController playerController;
    public VideoPlayer ohNoVideo;
    public HintUI hintUI;
    public TodoList todoList;

    public string[] words;
    public string[] waitWords;

    public float typingSpeed = 0.05f;
    public float[] waitTimes = { 20f, 40f, 60f, 120f };

    private bool Started;
    private bool Finished;

    void Start()
    {
        DialogueFlow.Clear(HidisText);
        hiCamera.SetActive(false);
        HiCanvas.SetActive(false);
        ohNoVideo.loopPointReached += VideoFinished;
    }

    void Update()
    {
        if (!Started &&
            Finished &&
            mainCameraRef.activeInHierarchy &&
            playerController.enabled)
        {
            StartCoroutine(HiSpeaking());
        }
    }

    void VideoFinished(VideoPlayer vp)
    {
        Finished = true;
    }

    IEnumerator HiSpeaking()
    {
        Started = true;

        if (hintUI != null)
            hintUI.BeginHidis(this);

        DialogueFlow.SwitchCamera(
            mainCameraRef, hiCamera,
            playerController, true);

        HiCanvas.SetActive(true);

        foreach (string line in words)
        {
            HidisAnim.Play("HiSpeak", 0, 0f);

            yield return DialogueFlow.TypeText(
                HidisText, line, typingSpeed);

            HidisAnim.Play("HiIdle", 0, 0f);

            float startTime = Time.time;
            int waitIndex = 0;
            Coroutine waitDialogue = null;

            yield return null;

            while (!DialogueFlow.ContinuePressed(true))
            {
                if (waitIndex < waitTimes.Length &&
                    Time.time - startTime >= waitTimes[waitIndex])
                {
                    if (waitDialogue != null)
                        StopCoroutine(waitDialogue);

                    if (waitIndex < waitWords.Length)
                        waitDialogue = StartCoroutine(WaitSpeak(waitIndex));

                    waitIndex++;
                }

                yield return null;
            }

            if (waitDialogue != null)
                StopCoroutine(waitDialogue);
        }

        ConEnd();
    }

    IEnumerator WaitSpeak(int index)
    {
        HidisAnim.Play("Wait" + (index + 1), 0, 0f);

        yield return DialogueFlow.TypeText(
            HidisText, waitWords[index], typingSpeed);

        HidisAnim.Play("HiIdle", 0, 0f);
    }

    void ConEnd()
    {
        DialogueFlow.Clear(HidisText);
        HiCanvas.SetActive(false);
        HidisAnim.Play("HiIdle", 0, 0f);

        DialogueFlow.SwitchCamera(
            mainCameraRef, hiCamera,
            playerController, false);

        if (hintUI != null)
            hintUI.EndInteraction(this);

        if (todoList != null)
            todoList.MarkTalked(this);
    }

    void OnDestroy()
    {
        if (ohNoVideo != null)
            ohNoVideo.loopPointReached -= VideoFinished;
    }
}