using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject startMenu;
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        Player.transform.position = spawnPoint.transform.position;

        startMenu.SetActive(false);
    }
}
