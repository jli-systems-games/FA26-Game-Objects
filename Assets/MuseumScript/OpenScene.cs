using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class OpenScene : MonoBehaviour
{
    public Animator MedoAnim;
    public TMP_Text MedoText;
    public float typingSpeed = 0.05f;
    public float txtTime = 2f;

    public Button StartButton;
    public GameObject BlackScreen;

    public AudioSource soundSource;
    public AudioClip appearSound;
    public AudioClip bounceSound;
    public AudioClip switchSound;

    public float waitTime = 10f;

    private bool clicked;

    void Start()
    {
        MedoText.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
        BlackScreen.SetActive(false);
        StartButton.onClick.AddListener(LetsGo);
        StartCoroutine(Opening());
    }

    IEnumerator Opening()
    {
        if (appearSound != null)
            soundSource.PlayOneShot(appearSound);

        yield return PlayAnimation("open1");
        yield return new WaitForSeconds(txtTime);
        yield return PlayAnimation("open2");
        soundSource.PlayOneShot(bounceSound);
        yield return new WaitForSeconds(txtTime);

        yield return DialogueFlow.TypeText(
            MedoText, "A new visitor!", typingSpeed);

        yield return new WaitForSeconds(txtTime);

        yield return DialogueFlow.TypeText(
            MedoText, "Welcome! Welcome!", typingSpeed);

        yield return new WaitForSeconds(txtTime);

        yield return PlayAnimation("open3");

        MedoAnim.Play("open4", 0, 0f);

        yield return DialogueFlow.TypeText(
            MedoText,
            "I want to show you my artworks!",
            typingSpeed);

        yield return new WaitForSeconds(txtTime);

        MedoText.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(true);

        float startTime = Time.time;

        while (!clicked && Time.time - startTime < waitTime)
        {
            yield return null;
        }

        StartButton.interactable = false;

        if (!clicked)
        {
            StartButton.gameObject.SetActive(false);

            yield return DialogueFlow.TypeText(
                MedoText,
                "Actually, you don't get a choice!",
                typingSpeed);

            yield return new WaitForSeconds(txtTime);
        }

        BlackScreen.SetActive(true);

        if (switchSound != null)
        {
            soundSource.PlayOneShot(switchSound);
            yield return new WaitForSeconds(switchSound.length);
        }
        else
        {
            yield return new WaitForSeconds(txtTime);
        }

        SceneManager.LoadScene("OCMuseum");
    }

    IEnumerator PlayAnimation(string animationName)
    {
        int state = Animator.StringToHash(
            MedoAnim.GetLayerName(0) + "." + animationName);

        MedoAnim.Play(state, 0, 0f);

        yield return null;

        yield return new WaitUntil(() =>
            MedoAnim.GetCurrentAnimatorStateInfo(0).fullPathHash == state &&
            MedoAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    public void LetsGo()
    {
        clicked = true;
    }
}