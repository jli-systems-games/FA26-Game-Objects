using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHurt : MonoBehaviour
{
    public List<AudioClip> hurtSounds; // List of hurt sounds to play when the player gets hurt
    public GameObject[] dialogueBoxes; // Array of dialogue boxes to display when the player gets hurt
    public float displayTime = 2f; 

    AudioSource hurtAudioSource;
    Coroutine hideCoroutine;

    void Start()
    {
        hurtAudioSource = GetComponent<AudioSource>(); // Get the AudioSource attached to this GameObject

        foreach (GameObject box in dialogueBoxes)
        {
            box.SetActive(false);
        }
    }

    public void RandomDisplay() //
    {
        if (dialogueBoxes.Length > 0)
        {
            foreach (GameObject box in dialogueBoxes)
            {
                box.SetActive(false);
            }

            int randomNumber = Random.Range(0, dialogueBoxes.Length); // Get a random index for the dialogue boxes
            dialogueBoxes[randomNumber].SetActive(true);

            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            hideCoroutine = StartCoroutine(HideDialogue());
        }

        if (hurtSounds.Count > 0 && hurtAudioSource != null) // Check if there are hurt sounds and the AudioSource is not null
        {
            AudioClip randomSound = hurtSounds[Random.Range(0, hurtSounds.Count)];
            hurtAudioSource.PlayOneShot(randomSound);
        }
    }

    IEnumerator HideDialogue()
    {
        yield return new WaitForSeconds(displayTime); // hide the dialogue box after 2s

        foreach (GameObject box in dialogueBoxes)
        {
            box.SetActive(false);
        }
        hideCoroutine = null;
    }
}
