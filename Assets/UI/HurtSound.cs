using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HurtSound : MonoBehaviour
{
    public TMP_Text hurtText;
   
    public List<AudioClip> hurtSounds;
    public List<string> hurtNames;

    // Start is called before the first frame update
    void Start()
    {
        hurtText = GameObject.Find("Hurt Text").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayARandomHurtText() 
    {
        print("You have called on this function");
        hurtText.text = hurtNames[Random.Range(0, hurtSounds.Count)];
        AudioSource hurtAudioSource = GetComponent<AudioSource>();
        hurtAudioSource.clip = hurtSounds[Random.Range(0, hurtSounds.Count)];
        hurtAudioSource.Play();
    }
}
