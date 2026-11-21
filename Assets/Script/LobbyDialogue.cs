using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LobbyDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public AudioSource audioSource;
    public TMP_Text dialogueText;

    [TextArea(2, 5)]
    public string[] dialogue = new string[10];

    private int currentLine = 0;
    private bool finished = false;
    private bool isTyping = false;

    public float typingSpeed = 0.03f;

    void Start()
    {
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (finished)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Loading Museum!");
                SceneManager.LoadScene("hallway");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            audioSource.Play(); 

            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogue[currentLine];
                isTyping = false;
            }
            else
            {
                currentLine++;

                if (currentLine < dialogue.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    finished = true;
                    dialogueText.text = "PRESS SPACE TO ENTER THE MUSEUM";
                }
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogue[currentLine])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
