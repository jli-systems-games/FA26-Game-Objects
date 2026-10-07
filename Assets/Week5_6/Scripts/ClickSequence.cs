using UnityEngine;
using UnityEngine.UI;

public class ClickSequence : MonoBehaviour
{
    [Header("Images shown in order")]
    public GameObject[] stages;

    [Header("Keep earlier images visible")]
    public bool keepPreviousStages = false;

    [Header("Invisible button covering the screen")]
    public Button clickButton;

    [Header("Animator on the final object")]
    public Animator finalAnimator;

    private int currentStage = 0;

    void Start()
    {
        // Only show the first stage at the beginning
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(i == 0);
        }
    }

    public void NextStage()
    {
        if (currentStage >= stages.Length - 1)
            return;

        if (!keepPreviousStages)
        {
            stages[currentStage].SetActive(false);
        }

        currentStage++;
        stages[currentStage].SetActive(true);

        // Play animation when the final stage appears
        if (currentStage == stages.Length - 1)
        {
            if (finalAnimator != null)
            {
                finalAnimator.SetTrigger("Play");
            }

            if (clickButton != null)
            {
                clickButton.interactable = false;
            }
        }
    }
}