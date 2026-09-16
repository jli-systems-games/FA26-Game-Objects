using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RandomText : MonoBehaviour
{
    public List<string> randomPhrases;

    public List<Sprite> randomImages;
    public Image spriteDisplay;

    [TextArea(2,2)]
    public List<string> dialogueText;
    public int dialogueTracker = 0;

    public List<int> numbersList;
    public TMP_Text phraseText;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        phraseText.text = dialogueText[dialogueTracker];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RandomizeText()  
    {
        phraseText.text = randomPhrases[Random.Range(0, randomPhrases.Count)];
    }
    public void DialogueDemo() 
    {
        
        if (dialogueTracker >= dialogueText.Count) 
        {
            dialogueTracker = 0;
            phraseText.text = dialogueText[dialogueTracker];
        }
        else 
        {
            dialogueTracker += 1;
            phraseText.text = dialogueText[dialogueTracker];
        }
        
    }
    public void RandomizeImage() 
    { 
        spriteDisplay.sprite = randomImages[Random.Range(0, randomImages.Count)];
    }

}
