using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{

    public TextMeshProUGUI dialogueText;
    public string[] lines;
    public float textPrintSpeed;

    public Image leftSprite;
    public Image rightSprite;
    public Sprite[] char1;
    public Sprite[] char2;

    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();

        // int randomLeft = Random.Range(0, char1.Length);
        // int randomRight = Random.Range(0, char2.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();

                int randomLeft = Random.Range(0, char1.Length);
                int randomRight = Random.Range(0, char2.Length);

                leftSprite.sprite = char1[randomLeft];
                rightSprite.sprite = char2[randomRight];
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textPrintSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("lines are done");
            SceneManager.LoadScene("week5&6InteractiveImage"); 
            //write exact name of scene WITHIN QUOTATIONS
        }
    }
}
