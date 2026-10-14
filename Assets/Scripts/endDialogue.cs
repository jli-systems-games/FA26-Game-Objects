using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class endDialogue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject targetCanvas;
    public GameObject trophy;

    public bool isInteracting;
    public List<string> dialogueText;
    public int dialogueTracker = 1;
    public TMP_Text dialogueTextObj;



    void Start()
    {
        dialogueTextObj.text = dialogueText[dialogueTracker];
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("space pressed");

                if (dialogueTracker >= dialogueText.Count - 1)
                {
                    Debug.Log("back to first dialogue");
                    dialogueTracker = 0;
                    dialogueTextObj.text = dialogueText[dialogueTracker];
                }
                else
                {
                    Debug.Log("next dialogue");
                    dialogueTracker += 1;
                    dialogueTextObj.text = dialogueText[dialogueTracker];
                }
            }

            if (dialogueTracker == 2)
            {
                if (trophy != null)
                { 
                    trophy.SetActive(true);
                }
                
            }
            
        }
    }





    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            Debug.Log("Player collided! Activating the Canvas.");


            if (targetCanvas != null)
            {
                targetCanvas.SetActive(true);

                isInteracting = true;
            }
            else
            {
                Debug.LogWarning("Target Canvas is missing! Assign it in the Inspector.");
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    Debug.Log("space pressed");

            //    if (dialogueTracker >= dialogueText.Count - 1)
            //    {
            //        Debug.Log("back to first dialogue");
            //        dialogueTracker = 0;
            //        dialogueTextObj.text = dialogueText[dialogueTracker];
            //    }
            //    else
            //    {
            //        Debug.Log("next dialogue");
            //        dialogueTracker += 1;
            //        dialogueTextObj.text = dialogueText[dialogueTracker];
            //    }

            //}
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player left! Deactivating canvas");

        targetCanvas.SetActive(false);

        isInteracting = false;
    }


}
