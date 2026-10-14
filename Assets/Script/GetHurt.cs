using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHurt : MonoBehaviour
{
    public List<AudioClip> hurtSounds;
    public GameObject[] dialogueBoxes;
    public float displayTime = 2f;

    AudioSource hurtAudioSource;
    Coroutine hideCoroutine;

    void Start()
    {
        hurtAudioSource = GetComponent<AudioSource>();

        foreach (GameObject box in dialogueBoxes)
        {
            box.SetActive(false);
        }
    }

    public void DisplayARandomHurtText()
    {
        if (dialogueBoxes.Length > 0)
        {
            foreach (GameObject box in dialogueBoxes)
            {
                box.SetActive(false);
            }

            int randomNumber = Random.Range(0, dialogueBoxes.Length);
            dialogueBoxes[randomNumber].SetActive(true);

            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            hideCoroutine = StartCoroutine(HideDialogue());
        }

        if (hurtSounds.Count > 0 && hurtAudioSource != null)
        {
            AudioClip randomSound = hurtSounds[Random.Range(0, hurtSounds.Count)];
            hurtAudioSource.PlayOneShot(randomSound);
        }
    }

    IEnumerator HideDialogue()
    {
        yield return new WaitForSeconds(displayTime);

        foreach (GameObject box in dialogueBoxes)
        {
            box.SetActive(false);
        }

        hideCoroutine = null;
    }
}
