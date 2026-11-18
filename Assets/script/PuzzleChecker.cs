using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public static PuzzleChecker Instance;
    private int[,] answer = new int[3, 3]
    {
        {0, -1, 1},
        {-1, 2, -1},
        {3, -1, 4}
    };
    public PuzzlePanel puzzlePanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CheckAllMatches()
    {
        bool allCorrect = true;
        Debug.Log("=====开始校验拼图=====");
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Transform grid = transform.Find($"Grid_{row}_{col}");
                if (grid == null)
                {
                    Debug.LogError($"❌ 找不到格子 Grid_{row}_{col}！名字错 / 嵌套了");
                    allCorrect = false;
                    continue;
                }
                int targetId = answer[row, col];
                Debug.Log($"格子 {grid.name} 目标ID：{targetId}");

                if (targetId == -1)
                {
                    if (grid.childCount > 0)
                    {
                        Debug.Log($"❌ 空白格子放了东西！");
                        allCorrect = false;
                    }
                    continue;
                }

                if (grid.childCount == 0)
                {
                    Debug.Log($"❌ 格子 {grid.name} 是空的");
                    allCorrect = false;
                    continue;
                }
                DraggableIcon icon = grid.GetChild(0).GetComponent<DraggableIcon>();
                if (icon.iconID != targetId)
                {
                    Debug.Log($"❌ 格子 {grid.name} 里面图标ID不对，当前:{icon.iconID} 目标:{targetId}");
                    allCorrect = false;
                }
                else
                {
                    Debug.Log($"✅ 格子 {grid.name} 图标正确");
                }
            }
        }
        if (allCorrect)
        {
            Debug.Log("🎉 拼图全部正确！开门");
            puzzlePanel.PuzzleComplete();
        }
        else
        {
            Debug.Log("❌ 拼图还没全部放对");
        }
    }
}