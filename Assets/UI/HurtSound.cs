using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HurtSound : MonoBehaviour
{
    private GameObject hurtTextobj;
    public TMP_Text hurtText;
    public TMP_Text playerHealthDisplay;
    public Player playerScriptRef;

    public List<AudioClip> hurtSounds;
    public List<string> hurtNames;

    // Start is called before the first frame update
    void Start()
    {
        hurtTextobj = GameObject.Find("HurtText");
        hurtText = hurtTextobj.GetComponent<TMP_Text>();
        playerScriptRef = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayARandomHurtText() 
    {
        print("You have called on this function");
        hurtText.text =  hurtNames[Random.Range(0, hurtSounds.Count)];
        playerHealthDisplay.text = playerScriptRef.playerHealth.ToString();
        
        AudioSource hurtAudioSource = GetComponent<AudioSource>();
        hurtAudioSource.clip = hurtSounds[Random.Range(0, hurtSounds.Count)];
        hurtAudioSource.Play();
    }
}
