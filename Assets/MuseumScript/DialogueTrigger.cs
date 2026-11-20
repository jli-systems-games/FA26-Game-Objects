using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string words;

    public GameObject mainCameraRef;
    public GameObject switchCamera;
    public Animator cameraAnimator;
    public string cameraAnimation;

    public PlayerController playerController;
    public TodoList todoList;
    public HintUI hintUI;

    public float dialogueDelay = 1f;
    public float typingSpeed = 0.05f;

    private bool playerTrigger;
    private bool isTalking;

    void Start()
    {
        switchCamera.SetActive(false);
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StopAllCoroutines();
                ConEnd();
            }
        }
        else if (playerTrigger &&
                 playerController.enabled &&
                 mainCameraRef.activeInHierarchy &&
                 Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Talk());
        }
    }

    IEnumerator Talk()
    {
        isTalking = true;

        if (hintUI != null)
            hintUI.BeginInteraction(this);

        DialogueFlow.SwitchCamera(
            mainCameraRef, switchCamera,
            playerController, true);

        if (cameraAnimator != null &&
            !string.IsNullOrEmpty(cameraAnimation))
        {
            cameraAnimator.Play(cameraAnimation, 0, 0f);
        }

        yield return new WaitForSeconds(dialogueDelay);

        dialogueBox.SetActive(true);

        yield return DialogueFlow.TypeText(
            dialogueText, words, typingSpeed);

        yield return DialogueFlow.WaitNext();

        if (todoList != null)
            todoList.MarkTalked(this);

        ConEnd();
    }

    void ConEnd()
    {
        DialogueFlow.Clear(dialogueText);
        dialogueBox.SetActive(false);

        DialogueFlow.SwitchCamera(
            mainCameraRef, switchCamera,
            playerController, false);

        isTalking = false;

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

            if (isTalking)
            {
                StopAllCoroutines();
                ConEnd();
            }
        }
    }
}