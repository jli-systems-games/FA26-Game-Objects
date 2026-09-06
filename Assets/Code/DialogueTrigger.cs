using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string words;
    private bool playerTrigger;

// Hide UI when click start
    void Start()
    {
        dialogueBox.SetActive(false);
    }

// Show UI when player is nearby the NPC and press E
    void Update()
    {
        if (playerTrigger && Input.GetKeyDown(KeyCode.E))
        {
            dialogueText.text = words;
            dialogueBox.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTrigger = true;
        }
    }

// Hide UI when player is away from the NPC
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTrigger = false;
            dialogueBox.SetActive(false);
        }
    }
}