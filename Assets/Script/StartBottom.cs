using UnityEngine;

public class StartBottom : MonoBehaviour
{
    void OnEnable()
    {
        if (MainLogic.scene >= 1 && MainLogic.scene <= 5)
         Time.timeScale = 0f;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
