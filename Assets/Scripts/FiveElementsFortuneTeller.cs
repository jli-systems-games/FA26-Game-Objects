using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FiveElementsFortuneTeller : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text elementNameText;
    public TMP_Text fortuneText;
    public TMP_Text buttonText;
    public Image elementImage;

    [Header("Fortune Content")]
    [TextArea(3, 6)]
    public List<string> fortunes = new List<string>();

    public List<string> elementNames = new List<string>();
    public List<Sprite> elementImages = new List<Sprite>();

    private int lastFortuneIndex = -1;

    public void GenerateFortune()
    {
        if (fortunes.Count == 0)
        {
            fortuneText.text = "No fortunes have been added.";
            return;
        }

        int randomIndex = Random.Range(0, fortunes.Count);

        if (fortunes.Count > 1)
        {
            while (randomIndex == lastFortuneIndex)
            {
                randomIndex = Random.Range(0, fortunes.Count);
            }
        }

        lastFortuneIndex = randomIndex;

        fortuneText.text = fortunes[randomIndex];

        if (randomIndex < elementNames.Count)
        {
            elementNameText.text = elementNames[randomIndex];
        }

        if (randomIndex < elementImages.Count)
        {
            elementImage.sprite = elementImages[randomIndex];
            elementImage.enabled = true;
        }

        buttonText.text = "Draw Again";
    }
}