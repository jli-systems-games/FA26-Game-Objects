using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonclick1 : MonoBehaviour
{
    public TMP_Text dialogue1;
    public string[] AltTitles = new string[4];

    public int listIndex = 0;

    public void ProceedDialogue()
    {
        
        if (listIndex < AltTitles.Length)
        {
            dialogue1.text = AltTitles[listIndex];
            listIndex++;
        }
        else
        {
            SceneManager.LoadScene("scene3");
        }
    }
}
