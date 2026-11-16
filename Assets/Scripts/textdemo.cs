using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Textdemo : MonoBehaviour
{
    // These are fields
    public List<string> dialogueText; // list of strings
    public int dialogueTracker = 0; // dialogueTracker is an int
    public List<int> numbersList;
    public TMP_Text phraseText; //phraseText is a TMP_Text

    private int inaccessibleInteger = -1;

    // These are functions
    private void Start()
    {
        phraseText.text = dialogueText[0];
        // int x; // x is an integer
        // x = 1; // x is 1 now
        // print(x);

        // x = 2; // x is 2 now
        // print(x);

        // x = 4; // x is 4 now
        // print(x);
        

        // // list format listOfStuff[indexOfThing];
        // List<int> listOfInts = new List<int>(5); // new list of 5 integers

        // //To reference this, use [] with a number like [2]
        // print(listOfInts[3]); // get the fourth element of the list

        // phraseText.text = dialogueText[dialogueTracker];
        // // Set phrase text to the dialogueTracker index of dialogueText;
        // print(phraseText.text);

        // Figure out how to trigger a random change using a button press
    }



    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            
            FortuneGenerator();
            
        }
       
    }
    public void FortuneGenerator ()
    {
        phraseText.text = dialogueText[Random.Range(0, dialogueText.Count)];
        
    }


}