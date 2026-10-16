using UnityEngine;
using TMPro;

public class HPUI : MonoBehaviour
{
    public int hp = 3;
    public GameObject gameOverPanel;

    TextMeshProUGUI text;

    void Start()   //set hp
    {

        text = GetComponent<TextMeshProUGUI>();
        text.text = "HP: " + hp; 
    }

    public void TakeDamage()
    {
     
        hp--;
        text.text = "HP: " + hp;

        if (hp <= 0)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
