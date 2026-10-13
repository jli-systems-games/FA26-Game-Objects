using UnityEngine;
using UnityEngine.UI;

public class MenuIconUpdater : MonoBehaviour
{
    [Header("Menu button images")]
    public Image[] menuImages;

    [Header("Images before playing")]
    public Sprite[] normalSprites;

    [Header("Images after completing")]
    public Sprite[] completedSprites;

    void Start()
    {
        UpdateMenuImages();
    }

    public void UpdateMenuImages()
    {
        for (int i = 0; i < menuImages.Length; i++)
        {
            if (GameProgress.IsGameCompleted(i))
            {
                menuImages[i].sprite = completedSprites[i];
            }
            else
            {
                menuImages[i].sprite = normalSprites[i];
            }
        }
    }

    public void ResetAllProgress()
    {
        GameProgress.ResetProgress();
        UpdateMenuImages();
    }
}