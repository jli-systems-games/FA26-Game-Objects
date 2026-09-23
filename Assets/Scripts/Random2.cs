using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

using TMPro;
public class Random2 : MonoBehaviour
{
    //public List<string> randomPhrases; 
    public List<string> randomPhrases;



    public TMP_Text phrasesObj; // Reference to the TMP_Text component




    void Update()
    {

    }

    public void RandomizeText()
    {
        phrasesObj.text = "the fruit is: " + randomPhrases[0];
        phrasesObj.text = randomPhrases[Random.Range(0, randomPhrases.Count)]; // Display a random phrase from the list
    }

}
