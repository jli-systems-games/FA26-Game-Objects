using UnityEngine;
using TMPro;
using System.Collections;

public class MuseumIntro : MonoBehaviour
{
    public GameObject MedoJ;
    public GameObject M3;
    public Animator MedoAnim;
    public TMP_Text MeIntro;
    public PlayerController playerController;

    public GameObject NotAMap;
    public GameObject MeNoMap;

    public GameObject musicHint;
    public GameObject mapHint;
    public Animator mapAnim;
    public GameObject todoHint;
    public GameObject todoPanel;

    public float typingSpeed = 0.05f;

    private bool waitingForMap;
    private bool mapOpened;
    private bool todoReady;

    void Start()
    {
        M3.SetActive(false);
        playerController.enabled = false;
        MeIntro.gameObject.SetActive(false);
        NotAMap.SetActive(false);
        MeNoMap.SetActive(false);
        musicHint.SetActive(false);
        mapHint.SetActive(false);
        todoHint.SetActive(false);

        if (todoPanel != null)
            todoPanel.SetActive(false);

        StartCoroutine(Intro());
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.T))
            return;

        if (waitingForMap)
        {
            mapOpened = true;
        }
        else if (todoReady &&
                 todoPanel != null &&
                 (todoPanel.activeSelf || playerController.enabled))
        {
            todoPanel.SetActive(!todoPanel.activeSelf);
            playerController.enabled = !todoPanel.activeSelf;
        }
    }

    IEnumerator Intro()
    {
        yield return Say("We're here! This is my OC Museum!");

        yield return Say(
            "This place brings together some of my artworks from the past few years!");

        musicHint.SetActive(true);
        yield return Say("Press M to switch the music!");

        yield return DialogueFlow.TypeText(MeIntro, "Press T to open the map!", typingSpeed);
        mapHint.SetActive(true);
        waitingForMap = true;

        yield return new WaitUntil(() => mapOpened);

        waitingForMap = false;
        MeIntro.gameObject.SetActive(false);

        NotAMap.SetActive(true);

        yield return new WaitForSeconds(1f);
        MeNoMap.SetActive(true);

        yield return Say(
            "Wait, this is a tiny museum. We don't actually need a map.");

        NotAMap.SetActive(false);
        MeNoMap.SetActive(false);

        if (mapAnim != null)
            yield return PlayAnimation(mapAnim, "MapOut");

        mapHint.SetActive(false);
        todoHint.SetActive(true);

        yield return Say("Press T to open your to-do list!");

        playerController.enabled = true;
        todoReady = true;

        yield return Say("You can move with WASD now!");
        yield return Say("Press Space to jump!");

        yield return PlayAnimation(MedoAnim, "MeIntro");

        yield return Say("When you get close to that big screen, press E to play a video!");

        yield return Say("That's all! Try interacting with different things!");

        yield return Say("You might discover a few little Easter eggs!");

        yield return Say("Have fun!");

        MeIntro.gameObject.SetActive(false);

        MedoJ.SetActive(false);
        M3.SetActive(true);
    }

    IEnumerator Say(string words)
    {
        yield return DialogueFlow.TypeText(
            MeIntro, words, typingSpeed);

        yield return DialogueFlow.WaitNext();
    }

    IEnumerator PlayAnimation(Animator animator, string animationName)
    {
        int state = Animator.StringToHash(
            animator.GetLayerName(0) + "." + animationName);

        animator.Play(state, 0, 0f);

        yield return null;

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).fullPathHash == state &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }
}