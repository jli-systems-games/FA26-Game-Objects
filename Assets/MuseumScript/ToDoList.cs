using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TodoList : MonoBehaviour
{
    public GameObject talkComplete;
    public GameObject viewComplete;
    public GameObject eatComplete;

    public MonoBehaviour[] npcs;
    public VideoPlayer[] videos;
    public EatItUp[] eats;

    public TMP_Text MeWord;
    public PlayerController playerController;
    public GameObject mainCameraRef;
    public GameObject todoPanel;
    public WorkIntro m3Intro;

    public float typingSpeed = 0.05f;

    private HashSet<MonoBehaviour> talked = new HashSet<MonoBehaviour>();
    private HashSet<VideoPlayer> watched = new HashSet<VideoPlayer>();
    private HashSet<EatItUp> eaten = new HashSet<EatItUp>();

    private bool finished;

    void Start()
    {
        talkComplete.SetActive(false);
        viewComplete.SetActive(false);
        eatComplete.SetActive(false);
    }

    public void MarkTalked(MonoBehaviour npc)
    {
        if (System.Array.IndexOf(npcs, npc) < 0)
            return;

        talked.Add(npc);
        CheckTasks();
    }

    public bool MarkWatched(VideoPlayer video)
    {
        if (System.Array.IndexOf(videos, video) < 0)
            return false;

        watched.Add(video);
        CheckTasks();
        return true;
    }

    public void MarkEaten(EatItUp npc)
    {
        if (System.Array.IndexOf(eats, npc) < 0)
            return;

        eaten.Add(npc);
        CheckTasks();
    }

    void CheckTasks()
    {
        talkComplete.SetActive(
            npcs.Length > 0 && talked.Count == npcs.Length);

        viewComplete.SetActive(
            videos.Length > 0 && watched.Count == videos.Length);

        eatComplete.SetActive(
            eats.Length > 0 && eaten.Count == eats.Length);

        if (!finished &&
            talkComplete.activeSelf &&
            viewComplete.activeSelf &&
            eatComplete.activeSelf)
        {
            finished = true;
            StartCoroutine(Finale());
        }
    }

    IEnumerator Finale()
    {
        yield return new WaitUntil(() =>
            playerController.enabled &&
            mainCameraRef.activeInHierarchy &&
            (todoPanel == null || !todoPanel.activeSelf));

        playerController.enabled = false;

        MeWord.transform.SetAsLastSibling();

        yield return DialogueFlow.TypeText(
            MeWord,
            "You did it! You've finished all the \"surface-level\" tasks! Haha!",
            typingSpeed);

        yield return DialogueFlow.WaitNext();

        yield return DialogueFlow.TypeText(
            MeWord,
            "Feel free to quit the game, but we'll meet again!",
            typingSpeed);

        yield return DialogueFlow.WaitNext();

        MeWord.gameObject.SetActive(false);
        playerController.enabled = true;

        if (m3Intro != null)
            m3Intro.UnlockExtraDialogue();
    }
}