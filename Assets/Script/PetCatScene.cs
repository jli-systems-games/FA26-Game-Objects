using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PetCatScene : MonoBehaviour
{
    public AudioSource soundEffect;
    public string nextScene;

    private SwapMusic musicManager;

    void Start()
    {
        musicManager = FindFirstObjectByType<SwapMusic>(); // Find the SwapMusic instance in the scene

        StartCoroutine(PlayPetCatScene());
    }

    IEnumerator PlayPetCatScene()
    {
        if (musicManager != null) 
        {
            musicManager.PauseMusic(); // Pause the background music
        }

        soundEffect.Play();
        yield return new WaitWhile(() => soundEffect.isPlaying);

        if (musicManager != null)
        {
            musicManager.ResumeMusic();
        }

        SceneManager.LoadScene(nextScene); // Load the next scene after the sound effect finishes
    }
}