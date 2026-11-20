using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    string[] currentLines;
    int currentLine = 0;
    bool dialogueActive = false;

    Action afterDialogue;

    public bool IsDialogueOpen => dialogueActive;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (dialogueActive && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    // things
    public void StartDialogue(string[] newLines)
    {
        StartDialogue(newLines, null);
    }

    // fall zone
    // 
    
    public void StartDialogue(string[] newLines, Action onDialogueFinished)
{
    Debug.Log("DialogueManager: StartDialogue was called.");

    if (dialogueActive)
    {
        Debug.LogWarning("DialogueManager: Dialogue is  active.");
        return;
    }

    if (newLines == null || newLines.Length == 0)
    {
        Debug.LogWarning("DialogueManager: No dialogue lines..");
        return;
    }

    if (dialoguePanel == null)
    {
        Debug.LogError("DialogueManager: Dialogue Panel not assigned");
        return;
    }

    if (dialogueText == null)
    {
        Debug.LogError("DialogueManager: Dialogue Text not assigned");
        return;
    }

    currentLines = newLines;
    currentLine = 0;
    dialogueActive = true;
    afterDialogue = onDialogueFinished;

    dialoguePanel.SetActive(true);
    //please work please
    dialogueText.gameObject.SetActive(true);
    dialogueText.enabled = true;
    dialogueText.text = currentLines[currentLine];

    Debug.Log("DialogueManager: Panel opened. First line: " + currentLines[0]);
}

    void NextLine()
    {
        currentLine++;

        if (currentLine < currentLines.Length)
        {
            dialogueText.text = currentLines[currentLine];
        }
        else
        {
            dialogueActive = false;
            dialoguePanel.SetActive(false);

            Action finished = afterDialogue;
            afterDialogue = null;

            if (finished != null)
            {
                finished.Invoke();
            }
        }
    }
}