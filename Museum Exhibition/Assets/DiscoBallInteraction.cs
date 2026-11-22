using UnityEngine;

public class DiscoBallInteraction : MonoBehaviour
{
    public GameObject welcomePanel;
    public Animator[] dancers;

    public RuntimeAnimatorController synchronizedDance;
    public RuntimeAnimatorController poseAnimation;

    private RuntimeAnimatorController[] originalDances;
    private bool hasOpenedWelcome = false;
    private int nextEvent = 0;

    void Start()
    {
        originalDances = new RuntimeAnimatorController[dancers.Length];

        for (int i = 0; i < dancers.Length; i++)
        {
            originalDances[i] = dancers[i].runtimeAnimatorController;
        }
    }

    void OnMouseDown()
    {
        if (!hasOpenedWelcome)
        {
            welcomePanel.SetActive(true);
            hasOpenedWelcome = true;
            return;
        }

        TriggerEvent();
    }

    void TriggerEvent()
    {
        switch (nextEvent)
        {
            case 0:
                foreach (Animator dancer in dancers)
                    dancer.speed = 0f;
                break;

            case 1:
                foreach (Animator dancer in dancers)
                {
                    dancer.runtimeAnimatorController = synchronizedDance;
                    dancer.speed = 1f;
                    dancer.Play(0, 0, 0f);
                }
                break;

            case 2:
                foreach (Animator dancer in dancers)
                {
                    dancer.runtimeAnimatorController = poseAnimation;
                    dancer.speed = 1f;
                    dancer.Play(0, 0, 0f);
                }
                break;

            case 3:
                for (int i = 0; i < dancers.Length; i++)
                {
                    dancers[i].runtimeAnimatorController = originalDances[i];
                    dancers[i].speed = 1f;
                }
                break;
        }

        nextEvent = (nextEvent + 1) % 4;
    }
}
