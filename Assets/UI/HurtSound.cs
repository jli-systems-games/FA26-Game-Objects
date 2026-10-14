using System.Collections;
using TMPro;
using UnityEngine;

public class HurtSound : MonoBehaviour
{
    TextMeshProUGUI text;
    Coroutine clearTextRoutine;

    public string[] hurtSounds;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (text != null)
            text.text = string.Empty;
    }

    public void DisplayARandomHurtText()
    {
        if (text == null || hurtSounds == null || hurtSounds.Length == 0)
            return;

        text.text = hurtSounds[Random.Range(0, hurtSounds.Length)];

        if (clearTextRoutine != null)
            StopCoroutine(clearTextRoutine);

        clearTextRoutine = StartCoroutine(ClearTextAfterDelay());
    }

    IEnumerator ClearTextAfterDelay()
    {
        yield return new WaitForSeconds(0.65f);
        text.text = string.Empty;
    }
}
