using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class TextScript : MonoBehaviour
{
    public List<string> randomPhrases;
    public TMP_Text phraseObj;
    public int numbersList;
    [TextArea(2,2)]
    public List<string> dialogueText;
    public int dialogueTracker = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public List<Sprite> randomImage;
    public Image spriteDisplay;
    void Start()
    {
        phraseObj.text = dialogueText[dialogueTracker];
    
    }

    // Update is called once per frame
    public void RandomizeText()
    {
        phraseObj.text = randomPhrases[Random.Range(0,randomPhrases.Count)];
        //phraseObj.text = numbersList[0].ToString();
        
    }
    public void DialogueDemo()
    {
        if (dialogueTracker >= dialogueText.Count - 1)
        {
            dialogueTracker = 0;
            phraseObj.text = dialogueText[dialogueTracker];
        }
        else
        {
     
            dialogueTracker += 1;
            phraseObj.text = dialogueText[dialogueTracker];
        }
    }
    public void randomizeImage()
    {
        spriteDisplay.sprite = randomImage[Random.Range(0, randomImage.Count)];
    }

}

