using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

using TMPro;
public class RandomText : MonoBehaviour
{
    //public List<string> randomPhrases; 
    public List<string> randomPhrases;
    public List<int> randomNumbers;

    [TextArea(3, 10)] // Optional: Set the size of the text area in the Inspector


    public TMP_Text phrasesObj; // Reference to the TMP_Text component




    void Update()
    {

    }

    public void RandomizeText()
    {
        phrasesObj.text = "this number is: " + randomPhrases[0];
        //phrasesObj.text = randomPhrases[6]; <-- display 6
        phrasesObj.text = randomPhrases[Random.Range(0, randomPhrases.Count)]; // Display a random phrase from the list
    }

}
