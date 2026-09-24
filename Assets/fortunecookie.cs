using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class fortunecookie : MonoBehaviour
{
    public TMP_Text fortuneText;

    public string[] fortunes =
    {
        "Something unexpected will happen on your commute...",
        "A small decision you will make will have vast consequences.",
        "Stay alert...",
        "Someone has very strong feelings about you...",
        "Be careful what you wish for...",
        "Stay inside tomorrow.",
        "A life altering event is soon to occur...",
        "Your lost socks will turn up unexpectedly.",
       
    };

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShowFortune();
        }
    }

    void ShowFortune()
    {
        int randomIndex = Random.Range(0, fortunes.Length);
        fortuneText.text = fortunes[randomIndex];
    }
}