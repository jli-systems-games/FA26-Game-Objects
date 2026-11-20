using System;
using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public static PuzzleChecker Instance;

    // iconID mapping: Eagle=0, Sun=1, Human=2, Wolf=3, Fire=4.
    // Required placements: Eagle (0,0), Sun (0,2), Human (1,1), Wolf (2,0), Fire (2,2).
    private readonly int[,] answer =
    {
        { 0, -1, 1 },
        { -1, 2, -1 },
        { 3, -1, 4 }
    };

    public PuzzlePanel puzzlePanel;
    private bool puzzleCompleted;

    private void Awake()
    {
        if (Instance == null || Instance == this)
            Instance = this;
        else
            Debug.LogError("More than one PuzzleChecker is active in the scene.", this);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void CheckAllMatches()
    {
        if (puzzleCompleted)
            return;

        bool allCorrect = true;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Transform grid = FindGrid(row, col);
                if (grid == null)
                {
                    Debug.LogError($"Puzzle grid_{row}_{col} was not found beneath {name}.", this);
                    allCorrect = false;
                    continue;
                }

                DraggableIcon[] icons = grid.GetComponentsInChildren<DraggableIcon>(true);
                int targetId = answer[row, col];

                if (targetId == -1)
                {
                    if (icons.Length != 0)
                        allCorrect = false;
                    continue;
                }

                if (icons.Length != 1 || icons[0].iconID != targetId)
                {
                    allCorrect = false;
                    continue;
                }
            }
        }

        if (!allCorrect)
            return;

        puzzleCompleted = true;
        if (puzzlePanel == null)
            puzzlePanel = FindObjectOfType<PuzzlePanel>();

        if (puzzlePanel != null)
            puzzlePanel.OnPuzzleCompleted();
        else
            Debug.LogError("Puzzle is solved, but no PuzzlePanel is assigned or present in the scene.", this);
    }

    public bool IsCorrectGridForIcon(int iconId, Transform grid)
    {
        if (grid == null)
            return false;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (answer[row, col] != iconId)
                    continue;

                Transform correctGrid = FindGrid(row, col);
                return correctGrid == grid;
            }
        }

        return false;
    }

    private Transform FindGrid(int row, int col)
    {
        string expectedName = $"Grid_{row}_{col}";
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        foreach (Transform candidate in transforms)
        {
            if (candidate != transform && string.Equals(candidate.name, expectedName, StringComparison.OrdinalIgnoreCase))
                return candidate;
        }

        return null;
    }
}
