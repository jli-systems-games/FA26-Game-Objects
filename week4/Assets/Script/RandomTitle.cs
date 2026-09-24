using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class RandomTitle : MonoBehaviour
{
    public TMP_Text TitleText;
    public UnityEngine.UI.Image DisplayImage;
    public string[] AltTitles = new string[4];
    public Sprite[] AltImages;
    public string newTitle;

    public void TitleChange()
    {
        if (AltTitles != null && AltTitles.Length >0)
        {
            newTitle = AltTitles[Random.Range(0, AltTitles.Length)];
            TitleText.text = newTitle;
        }

        if (AltImages != null && AltImages.Length > 0)
        {
            DisplayImage.sprite = AltImages [Random.Range(0, AltImages.Length)];
            DisplayImage.enabled = true;
        }
        
    }
}
