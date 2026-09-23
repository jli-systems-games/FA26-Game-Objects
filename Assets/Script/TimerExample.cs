using UnityEngine;
using TMPro;
public class TimerExample : MonoBehaviour
{
    public float timerNumber;
    public TMP_Text timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            timerNumber += Time.deltaTime;
            timerText.text = timerNumber.ToString("NO");
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            timerNumber += 0;
            timerText.text = timerNumber.ToString("NO");
        }
    }
}
