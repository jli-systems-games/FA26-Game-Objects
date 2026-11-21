using System.Collections;
using UnityEngine;
using TMPro;

public class BookDialogue : MonoBehaviour
{
    public GameObject floatingPrompt;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [TextArea(2, 5)]
    public string[] sentences;

    public float typingSpeed = 0.04f;

    private bool playerNear = false;
    private bool dialogueActive = false;
    private bool isTyping = false;

    private int sentenceIndex = 0;
    private Transform mainCamera;

    void Start()
    {
        floatingPrompt.SetActive(false);
        dialoguePanel.SetActive(false);

        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        if (floatingPrompt.activeSelf && mainCamera != null)
        {
            floatingPrompt.transform.LookAt(
                floatingPrompt.transform.position + mainCamera.forward,
                mainCamera.up
            );
        }

        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueActive == false)
            {
                StartDialogue();
            }
            else if (isTyping == false)
            {
                NextSentence();
            }
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        sentenceIndex = 0;

        floatingPrompt.SetActive(false);
        dialoguePanel.SetActive(true);

        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentences[sentenceIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextSentence()
    {
        if (sentenceIndex < sentences.Length - 1)
        {
            sentenceIndex++;
            StartCoroutine(TypeSentence());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);

        if (playerNear)
        {
            floatingPrompt.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            if (dialogueActive == false)
            {
                floatingPrompt.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            floatingPrompt.SetActive(false);

            if (dialogueActive)
            {
                StopAllCoroutines();
                isTyping = false;
                EndDialogue();
            }
        }
    }
}
