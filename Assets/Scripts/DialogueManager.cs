using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    string[] lines =
    {
        "Yo.",
        "Hows the weather today? I know it's not the season of ice cream anymore but welcome to...",
        "Oh yeah haven't introduced myself yet.",
        "I'm Green Seafoo's daughter, Seafood Gree.",
        "So, you're now in this little ice cream world."
    };

    int currentLine = 0;
    bool dialogueActive = false;

    // PlayerInteraction uses this to know whether dialogue is open.
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

    public void StartDialogue()
    {
        if (dialogueActive)
            return;

        dialoguePanel.SetActive(true);
        dialogueActive = true;
        currentLine = 0;
        dialogueText.text = lines[currentLine];
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            dialogueText.text = lines[currentLine];
        }
        else
        {
            dialogueActive = false;
            dialoguePanel.SetActive(false);
        }
    }
}