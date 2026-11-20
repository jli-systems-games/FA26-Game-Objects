using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    public void RestartGame()
    {
        // 重载第0号场景（你的游戏场景）
        SceneManager.LoadScene(0);
    }
}