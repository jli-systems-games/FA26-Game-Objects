using TMPro; 
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public TMP_Text titleText; 

    public string[] AltTitles = new string[8]; 
    public string newTitle; 
    


   
   

    public void TitleChange()
    {
        newTitle = AltTitles[Random.Range(0, AltTitles.Length)];

        titleText.text = newTitle;
    }
}
