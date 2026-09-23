
using TMPro;

using UnityEngine;

public class randomizewords : MonoBehaviour
{

    public TMP_Text words;
    public string[] AltTitles = new string[6];
    public string newTitle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void WordsChange()
    {
        newTitle = AltTitles[Random.Range(0, AltTitles.Length)];
        words.text = newTitle;

    }
}
