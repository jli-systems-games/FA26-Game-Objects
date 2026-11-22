
using UnityEngine;
using TMPro;

public class OverthinkingController : MonoBehaviour
{
    public GameObject overthinkingPanel;
    public TMP_Text overthinkingText;

    public string[] thoughts = {
        "Your entire camera roll just got posted publicly.", "Everyone hates you.",
    "You left your passport in the Uber.",
    "Your apartment is flooding and you're the last person to find out.",
    "You accidentally sent a screenshot of someone's conversation back to them.",
    "Your laptop just deleted your entire project.",
    "Your phone has been livestreaming from your front camera for the past hour.",
    "Your bank account is frozen and your rent is due tomorrow.",
    "You just found out your entire apartment has bedbugs.",
    "You accidentally sent a voice message while talking shit about someone.",
    "You left the stove on and you're three hours away from home."
    };

    public void OpenPanel()
    {
        overthinkingPanel.SetActive(true);
        NextThought();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NextThought()
    {
        int randomIndex = Random.Range(0, thoughts.Length);
        overthinkingText.text = thoughts[randomIndex];
    }

    public void ClosePanel()
    {
        overthinkingPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
