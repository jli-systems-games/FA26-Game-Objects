
using System.Collections;
using UnityEngine;
using TMPro;

public class GameCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    private Enemy[] enemies;

    void Start()
    {
       
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

       
        foreach (Enemy enemy in enemies)
        {
            enemy.canMove = false;
        }

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "Ready?";
        yield return new WaitForSeconds(1f);

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        countdownText.gameObject.SetActive(false);

       
        foreach (Enemy enemy in enemies)
        {
            enemy.canMove = true;
        }
    }
}
