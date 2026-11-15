using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public static float timeLeft;
    private float timeReveal;
    public GameObject winUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = 10;
        winUI.SetActive(false);

    }
    void Update()
    {
        if(timeLeft>=0){
        timeLeft = 10 - Time.time;
        timeReveal =  timeLeft  - (timeLeft % 1);
        GetComponent<TMP_Text>().text = timeReveal.ToString();
        }
        else{
            GetComponent<TMP_Text>().text = ""; 
            winUI.SetActive(true);
        }
    }
}
