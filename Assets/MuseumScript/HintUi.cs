using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HintUI : MonoBehaviour
{
    public GameObject interact;
    public Animator interactAnimator;
    public GameObject quit;
    public Animator quitAnimator;
    public GameObject fakeQuit;
    public GameObject ExInfo;

    public PlayerController playerController;
    public GameObject mainCameraRef;

    private HashSet<MonoBehaviour> nearby = new HashSet<MonoBehaviour>();
    private MonoBehaviour activeOwner;
    private Coroutine uiRoutine;
    private bool exInfoActive;

    private enum HintState
    {None, Interact, Quit, FakeQuit}

    private HintState currentState;

    void Awake()
    {
        interact.SetActive(false);
        quit.SetActive(false);

        if (fakeQuit != null)
            fakeQuit.SetActive(false);
    }

    void LateUpdate()
    {
        nearby.RemoveWhere(source =>
            source == null || !source.isActiveAndEnabled);

        bool showing = ExInfo != null && ExInfo.activeInHierarchy;

        if (showing != exInfoActive)
        {
            exInfoActive = showing;

            if (uiRoutine != null)
            {
                StopCoroutine(uiRoutine);
                uiRoutine = null;
            }

            if (exInfoActive)
            {
                interact.SetActive(false);
                quit.SetActive(false);
            }
            else
            {
                uiRoutine = StartCoroutine(SwitchUI(currentState));
            }
        }

        if (activeOwner == null)
            RefreshInteract();
    }

    public void EnterRange(MonoBehaviour source)
    {
        nearby.Add(source);
        RefreshInteract();
    }

    public void LeaveRange(MonoBehaviour source)
    {
        nearby.Remove(source);
        RefreshInteract();
    }

    public void BeginInteraction(MonoBehaviour source)
    {
        if (activeOwner != null && activeOwner != source)
            return;

        activeOwner = source;
        ChangeState(HintState.Quit);
    }

    public void BeginHidis(MonoBehaviour source)
    {
        activeOwner = source;
        ChangeState(HintState.FakeQuit);
    }

    public void EndInteraction(MonoBehaviour source)
    {
        if (activeOwner != source)
            return;

        activeOwner = null;
        RefreshInteract();
    }

    void RefreshInteract()
    {
        if (activeOwner != null)
            return;

        bool canInteract =
            nearby.Count > 0 &&
            (playerController == null || playerController.enabled) &&
            (mainCameraRef == null || mainCameraRef.activeInHierarchy);

        ChangeState(canInteract ? HintState.Interact : HintState.None);
    }

    void ChangeState(HintState nextState)
    {
        if (currentState == nextState)
            return;

        currentState = nextState;

        if (uiRoutine != null)
            StopCoroutine(uiRoutine);

        uiRoutine = null;

        if (!exInfoActive)
            uiRoutine = StartCoroutine(SwitchUI(nextState));
    }

    IEnumerator SwitchUI(HintState nextState)
    {
        if (nextState == HintState.FakeQuit)
        {
            interact.SetActive(false);
            quit.SetActive(false);

            if (fakeQuit != null)
                fakeQuit.SetActive(true);
        }
        else
        {
            if (fakeQuit != null)
                fakeQuit.SetActive(false);

            if (nextState == HintState.Interact)
            {
                yield return HideUI(quit, quitAnimator, "QuitExit");

                interact.SetActive(true);
                interactAnimator.Play("Interactive", 0, 0f);
                interactAnimator.Update(0f);
            }
            else if (nextState == HintState.Quit)
            {
                yield return HideUI(interact, interactAnimator, "InteractExit");

                quit.SetActive(true);
                quitAnimator.Play("Quit", 0, 0f);
                quitAnimator.Update(0f);
            }
            else
            {
                yield return HideUI(interact, interactAnimator, "InteractExit");
                yield return HideUI(quit, quitAnimator, "QuitExit");
            }
        }

        uiRoutine = null;
    }

    IEnumerator HideUI(GameObject target, Animator animator, string animationName)
    {
        if (target == null || !target.activeSelf)
            yield break;

        if (animator != null && animator.isActiveAndEnabled)
        {
            int state = Animator.StringToHash(
                animator.GetLayerName(0) + "." + animationName);

            if (animator.HasState(0, state))
            {
                animator.Play(state, 0, 0f);

                yield return null;

                yield return new WaitUntil(() =>
                    animator.GetCurrentAnimatorStateInfo(0).fullPathHash == state &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            }
        }

        target.SetActive(false);
    }
}