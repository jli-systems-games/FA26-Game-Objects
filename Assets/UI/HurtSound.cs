using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HurtSound : MonoBehaviour
{
    TextMeshProUGUI text;

    public string[] hurtSounds;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayARandomHurtText() {
        string randomHurtSound = hurtSounds[Random.Range(0, hurtSounds.Length)];
        text.text = randomHurtSound;
    }
}
