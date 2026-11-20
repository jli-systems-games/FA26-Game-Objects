using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("UI面板")]
    public GameObject PanelS;      // 主菜单面板（Hungry Puppy标题+Start按钮）
    public GameObject gameOverPanel;  // GameOver结束面板

    // Try Again按钮调用：关闭GameOver，打开主菜单
    public void BackToMenu()
    {
        gameOverPanel.SetActive(false);
        PanelS.SetActive(true);
    }

    // Start按钮调用：隐藏主菜单，开启游戏
    public void StartGame()
    {
        PanelS.SetActive(false);
    }
}