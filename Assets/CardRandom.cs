using UnityEngine;
using UnityEngine.UI;

public class CardRandomizer : MonoBehaviour
{
    [Header("UI Reference")]
    public Image cardDisplayImage; 

    [Header("Card Collection")]
    public Sprite[] tarotCards; 

    public void DisplayRandomCard()
    {

        int randomIndex = Random.Range(0, tarotCards.Length);

        cardDisplayImage.sprite = tarotCards[randomIndex];
    }
}
