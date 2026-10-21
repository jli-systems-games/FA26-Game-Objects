using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
public class CountDownTimer : MonoBehaviour
{
    public TMP_Text countDownText;
    public float timeLeft = 20f;
    public bool gameEnded;
    public CoinThreeD coinThreeDScript;
   
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0) 
        {
            timeLeft = 0;
            Debug.Log("Out of time.");
            Time.timeScale = 0;
            Restart();
        } 
        countDownText.text = "Time " + timeLeft;


    }
    void WinState(bool won) 
    {
        gameEnded = true;
        if ( coinThreeDScript.coinCollected == 10) 
        { 
            
        }
    }
    void Restart() 
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
