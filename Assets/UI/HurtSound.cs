using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HurtSound : MonoBehaviour
{
    TextMeshProUGUI text;
    public AudioSource hurtAudio;
    public string[] hurtSounds;


    void Start()   
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        
    }

    public  async void DisplayARandomHurtText() {
         hurtAudio.PlayOneShot(hurtAudio.clip);
         text.text = hurtSounds[Random.Range(0, hurtSounds.Length)];
         await System.Threading.Tasks.Task.Delay(400);
         text.text = "";
    }
}
