using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FishGenerator : MonoBehaviour
{
    public Sprite[] fishImages;
    public string[] fishTexts;

    public Image fishDisplay;
    public TMP_Text textDisplay;

     public void GenerateFish()
    {
        int randomFish = Random.Range(0, fishImages.Length);

        fishDisplay.sprite = fishImages[randomFish];
        textDisplay.text = fishTexts[randomFish];
    }
    
}
