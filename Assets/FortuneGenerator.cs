using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FortuneGenerator : MonoBehaviour
{
    public TMP_Text fortuneText;
    public Image fortuneImage;
    public AudioClip fortuneClip;

    [TextArea(2, 4)]
    public List<string> fortunes = new List<string>()
    {
        "The red lantern is lit, but no one is home.",
        "Do not look into the mirror after midnight.",
        "A paper figure has learned your name.",
        "Someone left embroidered shoes outside your door."
    };

    public List<Sprite> fortuneImages = new List<Sprite>();

    private int lastFortuneIndex = -1;
    private AudioSource fortuneAudio;

    private void Start()
    {
        fortuneImage.enabled = false;
        fortuneAudio = GetComponent<AudioSource>();
    }

    public void GenerateFortune()
    {
        int count = Mathf.Min(fortunes.Count, fortuneImages.Count);

        if (count == 0)
            return;

        int newIndex;

        do
        {
            newIndex = Random.Range(0, count);
        }
        while (count > 1 && newIndex == lastFortuneIndex);

        lastFortuneIndex = newIndex;

        fortuneText.text = fortunes[newIndex];
        fortuneImage.sprite = fortuneImages[newIndex];
        fortuneImage.enabled = true;

        if (fortuneAudio != null && fortuneClip != null)
        {
            fortuneAudio.PlayOneShot(fortuneClip);
        }
    }
}