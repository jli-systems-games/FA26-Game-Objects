using UnityEngine;
using TMPro;
using System.Collections;

public class WorkIntro : MonoBehaviour
{
    public GameObject M3;
    public SpriteRenderer M3Sprite;
    public GameObject mainCameraRef;
    public GameObject worksCamera;
    public Animator cameraAnimator;
    public PlayerController playerController;

    public TMP_Text MeIntro;
    public HintUI hintUI;
    public GameObject ExInfo;
    public Animator ExInfoAnim;
    public TodoList todoList;

    public float typingSpeed = 0.05f;

    private bool playerTrigger;
    private bool isTalking;
    private int extraStage;
    private Coroutine exInfoRoutine;

    void Start()
    {
        worksCamera.SetActive(false);
        MeIntro.gameObject.SetActive(false);
        ExInfo.SetActive(false);
    }

    void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ConEnd();
        }
        else if (playerTrigger &&
                 M3.activeInHierarchy &&
                 playerController.enabled &&
                 mainCameraRef.activeInHierarchy &&
                 Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Talk());
        }
    }

    public void UnlockExtraDialogue()
    {
        if (extraStage != 0)
            return;

        extraStage = 1;
        exInfoRoutine = StartCoroutine(ShowExInfo());
    }

    IEnumerator ShowExInfo()
    {
        ExInfo.SetActive(true);

        yield return new WaitForSeconds(5f);

        yield return PlayAnimation(ExInfoAnim, "NewExit");

        ExInfo.SetActive(false);
        exInfoRoutine = null;
    }

    IEnumerator Talk()
    {
        isTalking = true;
        if (hintUI != null)
            hintUI.BeginInteraction(this);

        playerController.enabled = false;

        if (extraStage == 1)
        {
            if (exInfoRoutine != null)
            {
                StopCoroutine(exInfoRoutine);
                exInfoRoutine = null;
            }

            ExInfo.SetActive(false);

            yield return SecretStory();
            extraStage = 2;
        }
        else if (extraStage == 2)
        {
            yield return SecretThanks();
            extraStage = 3;
        }
        else
        {
            mainCameraRef.SetActive(false);
            worksCamera.SetActive(true);

            yield return NormalTour();

            if (todoList != null)
                todoList.MarkTalked(this);
        }

        ConEnd();
    }

    IEnumerator NormalTour()
    {
        yield return PlayAnimation(cameraAnimator, "WorkView");

        yield return Say("These are some of my pixel artworks from the past!");

        M3Sprite.enabled = false;

        yield return PlayAnimation(cameraAnimator, "PotatoWalk");
        yield return Say("Potato Walk, 2022.");

        yield return PlayAnimation(cameraAnimator, "FaFlower");
        yield return Say("FaFlower, 2022.");

        yield return PlayAnimation(cameraAnimator, "MyDesk");
        yield return Say("My Desk, 2022.");

        yield return Say(
            "This was pretty much my first big pixel-art scene. My portfolio mentor actually noticed my talent because of it. Ah, those were the days!");

        yield return PlayAnimation(cameraAnimator, "Foxxi");
        yield return Say("Foxxi, 2023.");

        yield return PlayAnimation(cameraAnimator, "ShineFafa");
        yield return Say("ShineFafa, 2023.");

        yield return PlayAnimation(cameraAnimator, "SwingSix");
        yield return Say("SwingSix, 2024.");

        yield return Say(
            "I made this for a friend! I feel so bad for keeping her waiting almost six months!");

        yield return PlayAnimation(cameraAnimator, "ISGYSD");
        yield return Say("ISGYSD, 2025.");

        M3Sprite.enabled = true;
        worksCamera.SetActive(false);
        mainCameraRef.SetActive(true);

        yield return Say(
            "I know these names sound weird. I usually don't name my artworks. They're basically just variable names I made up on the spot!");
    }

    IEnumerator SecretStory()
    {
        yield return Say(
            "Wanna know why I look like this? Well...");

        yield return Say(
            "This is my online persona, since I don't really want to appear as a human on the internet.");

        yield return Say(
            "This is connected to some of my childhood experiences. I like birds, and when I was around ten years old, I used to keep some small parrots.");

        yield return Say(
            "So I named my WeChat account \"Experienced Old Hen.\" My classmates thought it was funny and started calling me \"Hen\" too.");

        yield return Say(
            "Later, when I was in high school, I changed my online name to \"Mendori,\" which means \"hen\" in Japanese.");

        yield return Say(
            "The last syllable, \"ri,\" sounds like the Chinese word for pear. So I combined these ideas and designed this character as my online identity.");

        yield return Say(
            "So, I'm not telling you my legal name here. Haha!");
    }

    IEnumerator SecretThanks()
    {
        yield return Say(
            "Back when I made those four games, my mentor helped me so much!");

        yield return Say(
            "My older sister even composed a short piece of music for each game!");

        yield return Say(
            "That was honestly one of the most exciting times of my life!");
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

        if (!animator.HasState(0, state))
            yield break;

        animator.Play(state, 0, 0f);

        yield return null;

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).fullPathHash == state &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    void ConEnd()
    {
        StopAllCoroutines();

        M3Sprite.enabled = true;
        MeIntro.text = "";
        MeIntro.gameObject.SetActive(false);

        worksCamera.SetActive(false);
        mainCameraRef.SetActive(true);

        playerController.enabled = true;
        isTalking = false;

        if (hintUI != null)
            hintUI.EndInteraction(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = true;

            if (hintUI != null)
                hintUI.EnterRange(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;
            if (hintUI != null)
                hintUI.LeaveRange(this);

            if (isTalking)
                ConEnd();
        }
    }
}