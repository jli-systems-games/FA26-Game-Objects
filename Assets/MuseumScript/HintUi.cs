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

    public PlayerController playerController;
    public GameObject mainCameraRef;

    private HashSet<MonoBehaviour> nearby = new HashSet<MonoBehaviour>();
    private MonoBehaviour activeOwner;
    private bool hidisTalking;
    private Coroutine interactExit;
    private Coroutine quitExit;

    void Awake()
    {
        if (interact != null) interact.SetActive(false);
        if (quit != null) quit.SetActive(false);
        if (fakeQuit != null) fakeQuit.SetActive(false);
    }

    void LateUpdate()
    {
        nearby.RemoveWhere(source => source == null || !source.isActiveAndEnabled);
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
        hidisTalking = false;
        HideInteract();
        ShowQuit();
    }

    public void BeginHidis(MonoBehaviour source)
    {
        activeOwner = source;
        hidisTalking = true;
        HideInteractNow();
        HideQuitNow();

        if (fakeQuit != null)
            fakeQuit.SetActive(true);
    }

    public void EndInteraction(MonoBehaviour source)
    {
        if (activeOwner != source)
            return;

        if (hidisTalking)
        {
            if (fakeQuit != null)
                fakeQuit.SetActive(false);
        }
        else
        {
            HideQuit();
        }

        activeOwner = null;
        hidisTalking = false;
        RefreshInteract();
    }

    void RefreshInteract()
    {
        bool canInteract = activeOwner == null && nearby.Count > 0 &&
                           (playerController == null || playerController.enabled) &&
                           (mainCameraRef == null || mainCameraRef.activeInHierarchy);

        if (canInteract)
            ShowInteract();
        else
            HideInteract();
    }

    void ShowInteract()
    {
        if (interact == null || interactAnimator == null)
            return;

        if (interact.activeSelf && interactExit == null)
            return;

        if (interactExit != null)
        {
            StopCoroutine(interactExit);
            interactExit = null;
        }

        interact.SetActive(true);
        interactAnimator.Play("Interactive", 0, 0f);
    }

    void HideInteract()
    {
        if (interact != null && interact.activeSelf && interactExit == null)
            interactExit = StartCoroutine(Hide(interact, interactAnimator, "InteractExit", true));
    }

    void HideInteractNow()
    {
        if (interactExit != null)
        {
            StopCoroutine(interactExit);
            interactExit = null;
        }

        if (interact != null)
            interact.SetActive(false);
    }

    void ShowQuit()
    {
        if (quit == null || quitAnimator == null)
            return;

        if (quit.activeSelf && quitExit == null)
            return;

        if (quitExit != null)
        {
            StopCoroutine(quitExit);
            quitExit = null;
        }

        quit.SetActive(true);
        quitAnimator.Play("Quit", 0, 0f);
    }

    void HideQuit()
    {
        if (quit != null && quit.activeSelf && quitExit == null)
            quitExit = StartCoroutine(Hide(quit, quitAnimator, "QuitExit", false));
    }

    void HideQuitNow()
    {
        if (quitExit != null)
        {
            StopCoroutine(quitExit);
            quitExit = null;
        }

        if (quit != null)
            quit.SetActive(false);
    }

    IEnumerator Hide(GameObject target, Animator animator, string stateName, bool isInteract)
    {
        if (animator != null)
        {
            int state = Animator.StringToHash(animator.GetLayerName(0) + "." + stateName);

            if (animator.HasState(0, state))
            {
                animator.Play(state, 0, 0f);
                yield return null;

                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                float speed = Mathf.Max(0.01f, Mathf.Abs(info.speed * animator.speed));

                if (info.fullPathHash == state)
                    yield return new WaitForSeconds(info.length / speed);
            }
        }

        target.SetActive(false);

        if (isInteract)
            interactExit = null;
        else
            quitExit = null;
    }
}