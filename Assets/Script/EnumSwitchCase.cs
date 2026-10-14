using TMPro;
using UnityEngine;

public class EnumSwitchCase : MonoBehaviour
{
    //EXAMPLE OF HOW TO USE AN ENUM.
    public enum  tonalMessages //STEP 1: Create the enum. In my case, I have different message tones I want to select.
    { 
        NeutralMessage,
        HappyMessage,
        SadMessage,
        TiredMessage

    }

    public tonalMessages messageTypes;//STEP 2: You can now use that enum as a variable. You can make it public/private than give it it's own unique name like any regular variable.

    public TMP_Text displayMessage;//Since I want to change the text on my canvas, I need to create a TMP_Text reference.
   
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //STEP 3: A 'switch/case' allows you to more cleanly check for certain conditions and execute code if they are met.
        // They are good for when you need to check for a lot of conditionals and you don't want to deal with a mess of nested 'if' statements.

        switch (messageTypes) //The 'switch' is the variable that is being watched for changes.
        {
            //the 'case' are the conditions where you want something to happen. The 'what' can be anything you want it to be. Just like a regular 'if' statement.
            case tonalMessages.HappyMessage:
                displayMessage.text = "I am so happy today!";
                break;
            case tonalMessages.NeutralMessage:
                displayMessage.text = "I feel whatever today";
                break;
            case tonalMessages.SadMessage:
                displayMessage.text = "I'm feeling kinda blue...";
                break;
            case tonalMessages.TiredMessage:
                displayMessage.text = "I want to go back to sleep.";
                break;
        }

        //BONUS: Enums can be used outside of 'switch case' statements. Here, a message prints to the console if the current enum is 'Happy Message'.
        if (messageTypes == tonalMessages.HappyMessage)
        {
            print("The current message is a happy one.");
        }
        
    }
}
