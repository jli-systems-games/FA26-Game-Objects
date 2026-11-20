using UnityEngine;
using TMPro;
using System.Collections;

public static class DialogueFlow
{
    public static IEnumerator TypeText(
        TMP_Text text, string fullText, float typingSpeed)
    {
        text.gameObject.SetActive(true);
        text.text = "";

        foreach (char c in fullText)
        {
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public static IEnumerator TypeText(
        GameObject box, TMP_Text text,
        string fullText, float typingSpeed)
    {
        box.SetActive(true);
        yield return TypeText(text, fullText, typingSpeed);
    }

    public static bool ContinuePressed(bool mouse = true)
    {
        return Input.GetKeyDown(KeyCode.E) ||
               (mouse && Input.GetMouseButtonDown(0));
    }

    public static IEnumerator WaitNext()
    {
        yield return null;
        yield return new WaitUntil(() => ContinuePressed());
    }

    public static IEnumerator Speak(
        GameObject box, TMP_Text text,
        string words, float typingSpeed)
    {
        yield return TypeText(box, text, words, typingSpeed);
        yield return WaitNext();
    }

    public static void Clear(TMP_Text text)
    {
        text.text = "";
        text.maxVisibleCharacters = int.MaxValue;
    }

    public static void SwitchCamera(
        GameObject mainCamera,
        GameObject dialogueCamera,
        PlayerController player,
        bool talking)
    {
        if (talking)
        {
            player.enabled = false;
            mainCamera.SetActive(false);
            dialogueCamera.SetActive(true);
        }
        else
        {
            dialogueCamera.SetActive(false);
            mainCamera.SetActive(true);
            player.enabled = true;
        }
    }

    public static void EndDialogue(
        GameObject box, TMP_Text text,
        GameObject mainCamera,
        GameObject dialogueCamera,
        PlayerController player)
    {
        Clear(text);
        box.SetActive(false);

        SwitchCamera(
            mainCamera, dialogueCamera, player, false);
    }

}