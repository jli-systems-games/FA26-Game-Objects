using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("UI Canvas")]
    public GameObject startMenuCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject winCanvas;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.28f;

    private bool gameStarted = false;
    private bool isPaused = false;
    private bool gameWin = false;

    public bool CursorShouldBeUnlocked =>
        (startMenuCanvas != null && startMenuCanvas.activeInHierarchy) ||
        (pauseMenuCanvas != null && pauseMenuCanvas.activeInHierarchy) ||
        (winCanvas != null && winCanvas.activeInHierarchy);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (startMenuCanvas != null) startMenuCanvas.SetActive(true);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (winCanvas != null) winCanvas.SetActive(false);

        // Load from Assets/Resources so the music works without a manual Inspector assignment.
        if (backgroundMusic == null)
            backgroundMusic = Resources.Load<AudioClip>("AncientMysteries");

        if (backgroundMusic != null)
        {
            AudioSource musicSource = GetComponent<AudioSource>();
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = musicVolume;
            musicSource.spatialBlend = 0f;
            if (!musicSource.isPlaying) musicSource.Play();
        }
        else
        {
            Debug.LogWarning("GameUIManager could not load background music 'AncientMysteries' from a Resources folder.");
        }

        if (CursorShouldBeUnlocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        if (CursorShouldBeUnlocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && gameStarted && !gameWin)
        {
            TogglePauseMenu();
        }
    }

    public void StartGame()
    {
        if (startMenuCanvas != null) startMenuCanvas.SetActive(false);
        gameStarted = true;
        Time.timeScale = 1;
        // 锁定鼠标，隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            // 暂停：解锁鼠标
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // 继续游戏：锁定鼠标
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToStartMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
        if (gameWin) return;
        gameWin = true;
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("GameUIManager.WinGame called, but winCanvas is not assigned.");
        }
        Time.timeScale = 0;
        // 胜利界面解锁鼠标，用来点Restart
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
