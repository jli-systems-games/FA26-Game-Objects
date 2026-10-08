using UnityEngine;
using TMPro;

public class buttonclick1 : MonoBehaviour
{
    public TMP_Text dialogue1;
    public string[] AltTitles = new string[4];

    public int listIndex = 0;

    public void ProceedDialogue()
    {
        dialogue1.text = AltTitles[listIndex];
        listIndex++;
    }
}
