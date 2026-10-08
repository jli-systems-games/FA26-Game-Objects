using UnityEngine;
using UnityEngine.UI;

public class HatSwitcher : MonoBehaviour
{
    public Image hatImage;
    public Sprite[] hats;
    public Animator hatAnimator;

    private int currentHat = 0;

    public void NextHat()
    {
        currentHat++;

        if (currentHat >= hats.Length)
        {
            currentHat = 0;
        }

        hatImage.sprite = hats[currentHat];
        hatAnimator.Play("hatdrop", 0, 0f);
    }

    public void PreviousHat()
    {
        currentHat--;

        if (currentHat < 0)
        {
            currentHat = hats.Length - 1;
        }

        hatImage.sprite = hats[currentHat];
        hatAnimator.Play("hatdrop", 0, 0f);
    }
}