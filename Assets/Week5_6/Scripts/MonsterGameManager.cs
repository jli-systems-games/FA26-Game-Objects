using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterGameManager : MonoBehaviour
{
    [Header("Three monsters in their fixed positions")]
    public Button[] monsters;

    [Header("Game Settings")]
    public float visibleTime = 1.2f;
    public float pauseTime = 0.4f;

    [Header("UI")]
    public TMP_Text scoreText;
    public GameObject completePanel;

    private bool[] caught;
    private int currentMonster = -1;
    private int caughtCount = 0;
    private bool canHit = false;

    void Start()
    {
        caught = new bool[monsters.Length];

        for (int i = 0; i < monsters.Length; i++)
        {
            int monsterIndex = i;

            monsters[i].onClick.AddListener(
                () => HitMonster(monsterIndex)
            );

            monsters[i].gameObject.SetActive(false);
        }

        completePanel.SetActive(false);
        UpdateScore();

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (caughtCount < monsters.Length)
        {
            currentMonster = GetRandomUncaughtMonster();

            monsters[currentMonster].gameObject.SetActive(true);
            canHit = true;

            float timer = 0f;

            while (timer < visibleTime && canHit)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            canHit = false;
            monsters[currentMonster].gameObject.SetActive(false);

            yield return new WaitForSeconds(pauseTime);
        }

        GameProgress.CompleteGame(3);
        completePanel.SetActive(true);
    }

    void HitMonster(int clickedMonster)
    {
        if (!canHit || clickedMonster != currentMonster)
            return;

        canHit = false;

        if (!caught[clickedMonster])
        {
            caught[clickedMonster] = true;
            caughtCount++;
        }

        monsters[clickedMonster].gameObject.SetActive(false);
        UpdateScore();
    }

    int GetRandomUncaughtMonster()
    {
        int index;

        do
        {
            index = Random.Range(0, monsters.Length);
        }
        while (caught[index]);

        return index;
    }

    void UpdateScore()
    {
        scoreText.text = caughtCount + " / " + monsters.Length;
    }

    public void RestartGame()
    {
        StopAllCoroutines();

        caught = new bool[monsters.Length];
        caughtCount = 0;
        currentMonster = -1;
        canHit = false;

        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].gameObject.SetActive(false);
        }

        completePanel.SetActive(false);
        UpdateScore();

        StartCoroutine(GameLoop());
    }
}