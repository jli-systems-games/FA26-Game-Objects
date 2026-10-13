using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPageManager : MonoBehaviour
{
    // 保留之前的游戏进度
    public void ContinueGame()
    {
        SceneManager.LoadScene("01_Menu");
    }

    // 清除全部进度，重新开始
    public void RestartGame()
    {
        GameProgress.ResetProgress();
        SceneManager.LoadScene("01_Menu");
    }
}