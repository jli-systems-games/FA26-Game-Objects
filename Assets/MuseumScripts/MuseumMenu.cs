using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Start screen + pause menu.
//  - Start screen shows when the game opens: title, short intro, controls, START.
//  - TAB (or P) opens the pause menu: RESUME, RESTART, EXIT. Controls are listed there too.
//  - While a menu is open the visitor cannot move, the mouse is free and time is paused (music keeps playing).
//  - EXIT quits on desktop; in a web browser (WebGL) it returns to the start screen, because a web page cannot close itself.
public class MuseumMenu : MonoBehaviour
{
    public MuseumVisitorController visitor;
    public MuseumExhibitInteractor interactor;
    public GameObject startScreen;
    public GameObject pauseMenu;

    static bool skipStartScreen;   // set by RESTART so the reloaded scene goes straight into the game
    public bool MenuOpen => (startScreen!=null && startScreen.activeSelf) || (pauseMenu!=null && pauseMenu.activeSelf);

    void Start()
    {
        if(pauseMenu!=null)pauseMenu.SetActive(false);
        if(skipStartScreen){skipStartScreen=false;Show(null);}
        else Show(startScreen);
    }

    void Update()
    {
        var keys=Keyboard.current;if(keys==null)return;
        bool toggle=keys.tabKey.wasPressedThisFrame||keys.pKey.wasPressedThisFrame;
        if(!toggle||(startScreen!=null && startScreen.activeSelf))return;
        if(pauseMenu.activeSelf)Resume();
        else if((interactor==null||!interactor.IsReading)&&(visitor==null||visitor.cameraOverride==null))Show(pauseMenu); // not while reading or using the telescope
    }

    void Show(GameObject menu)
    {
        if(startScreen!=null)startScreen.SetActive(menu==startScreen);
        if(pauseMenu!=null)pauseMenu.SetActive(menu==pauseMenu);
        bool open=menu!=null;
        Time.timeScale=open?0:1;
        if(interactor!=null)interactor.enabled=!open;
        if(visitor!=null){visitor.inputLocked=open;visitor.SetLocked(!open);}
    }

    // Button callbacks (wired in the Inspector by the build tool)
    public void StartGame()=>Show(null);
    public void Resume()=>Show(null);
    public void Restart(){Time.timeScale=1;skipStartScreen=true;SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);}
    public void Exit()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Time.timeScale=1;skipStartScreen=false;SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
#elif UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
#else
        Application.Quit();
#endif
    }
}
