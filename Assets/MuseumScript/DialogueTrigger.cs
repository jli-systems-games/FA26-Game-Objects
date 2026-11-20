using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string words;

    public PlayerController playerController;
    public TodoList todoList;
    public HintUI hintUI;

    public float typingSpeed = 0.05f;

    private bool playerTrigger;
    private bool isTalking;

    void Start()
    {
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ConEnd();
        }
        else if (playerTrigger &&
                 playerController.enabled &&
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

        dialogueBox.SetActive(true);

        yield return DialogueFlow.TypeText(
            dialogueText, words, typingSpeed);

        if (todoList != null)
            todoList.MarkTalked(this);

        yield return DialogueFlow.WaitNext();

        ConEnd();
    }

    void ConEnd()
    {
        StopAllCoroutines();

        DialogueFlow.Clear(dialogueText);
        dialogueBox.SetActive(false);

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
                ConEnd();
        }
    }
}