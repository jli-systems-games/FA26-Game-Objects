using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPlaying { get; private set; }

    const float WinTime = 30f;
    const int MaxActiveEnemies = 12;

    Player player;
    Enemy[] enemies;
    float elapsedTime;
    TextMeshProUGUI healthText;
    TextMeshProUGUI timerText;
    TextMeshProUGUI resultTitle;
    TextMeshProUGUI resultSubtitle;
    GameObject startPanel;
    GameObject resultPanel;
    Image damageFlash;
    AudioSource audioSource;
    AudioClip hitSound;
    AudioClip winSound;
    AudioClip loseSound;
    AudioSource ambientSource;
    Transform[] portalRings;
    SpriteRenderer[] driftingFragments;
    Vector3[] fragmentOrigins;
    float[] fragmentPhases;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CreateAutomatically()
    {
        if (FindFirstObjectByType<GameManager>() == null)
            new GameObject("GameManager").AddComponent<GameManager>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.38f;
        hitSound = CreateTone(180f, 0.11f, 0.45f);
        winSound = CreateTone(720f, 0.35f, 0.28f);
        loseSound = CreateTone(105f, 0.5f, 0.38f);

        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.playOnAwake = false;
        ambientSource.loop = true;
        ambientSource.volume = 0.045f;
        ambientSource.clip = CreateAmbientLoop();
    }

    void Start()
    {
        if (Camera.main != null)
            Camera.main.backgroundColor = new Color(0.008f, 0.006f, 0.025f);

        player = FindFirstObjectByType<Player>();
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        for (int i = MaxActiveEnemies; i < enemies.Length; i++)
            enemies[i].gameObject.SetActive(false);

        CreateLiminalWorld();
        BuildInterface();
        ShowStartScreen();
    }

    void Update()
    {
        AnimateLiminalWorld();

        if (!IsPlaying)
        {
            if (((startPanel != null && startPanel.activeSelf) ||
                 (resultPanel != null && resultPanel.activeSelf)) &&
                (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
                StartGame();
            return;
        }

        elapsedTime += Time.deltaTime;
        int secondsLeft = Mathf.Max(0, Mathf.CeilToInt(WinTime - elapsedTime));
        timerText.text = "PORTAL  " + secondsLeft + "s";

        if (elapsedTime >= WinTime)
            EndGame(true);
    }

    void ShowStartScreen()
    {
        IsPlaying = false;
        startPanel.SetActive(true);
        resultPanel.SetActive(false);
        SetHudVisible(false);
        ambientSource.Stop();
    }

    public void StartGame()
    {
        elapsedTime = 0f;
        startPanel.SetActive(false);
        resultPanel.SetActive(false);
        SetHudVisible(true);

        if (player != null)
        {
            player.ResetPlayer();
            UpdateHealth(player.CurrentHealth, player.MaxHealth);
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject.activeSelf)
                enemies[i].ResetEnemy(true);
        }

        IsPlaying = true;
        timerText.text = "PORTAL  30s";
        ambientSource.Play();
    }

    public void OnPlayerDamaged(int currentHealth, int maxHealth)
    {
        UpdateHealth(currentHealth, maxHealth);
        audioSource.PlayOneShot(hitSound);
        StartCoroutine(FlashDamage());
    }

    public void EndGame(bool won)
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;
        ambientSource.Stop();
        SetHudVisible(false);
        resultPanel.SetActive(true);
        resultTitle.text = won ? "PORTAL STABILIZED" : "SIGNAL LOST";
        resultTitle.color = won ? new Color(0.35f, 1f, 0.92f) : new Color(1f, 0.25f, 0.66f);
        resultSubtitle.text = won
            ? "You crossed the space between signals."
            : "Your form dissolved into the static.";
        audioSource.PlayOneShot(won ? winSound : loseSound);
    }

    void UpdateHealth(int currentHealth, int maxHealth)
    {
        string hearts = string.Empty;
        for (int i = 0; i < maxHealth; i++)
            hearts += i < currentHealth ? "♥ " : "x ";
        healthText.text = "BODY  " + hearts;
    }

    void SetHudVisible(bool visible)
    {
        healthText.gameObject.SetActive(visible);
        timerText.gameObject.SetActive(visible);
    }

    IEnumerator FlashDamage()
    {
        damageFlash.color = new Color(1f, 0.02f, 0.58f, 0.32f);
        yield return new WaitForSeconds(0.08f);
        damageFlash.color = Color.clear;
    }

    void BuildInterface()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        if (scaler == null)
            scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(800f, 600f);
        scaler.matchWidthOrHeight = 0.5f;

        if (FindFirstObjectByType<EventSystem>() == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        GameObject root = CreateUiObject("Week7Interface", canvas.transform);
        Stretch(root.GetComponent<RectTransform>());

        damageFlash = CreatePanel("DamageFlash", root.transform, Color.clear).GetComponent<Image>();
        damageFlash.raycastTarget = false;

        healthText = CreateText("Health", root.transform, "BODY  ♥♥♥", 25f, FontStyles.Bold);
        SetRect(healthText.rectTransform, new Vector2(18f, -18f), new Vector2(330f, 55f), new Vector2(0f, 1f));
        healthText.alignment = TextAlignmentOptions.TopLeft;
        healthText.color = new Color(1f, 0.28f, 0.72f);

        timerText = CreateText("Timer", root.transform, "PORTAL  30s", 22f, FontStyles.Bold);
        SetRect(timerText.rectTransform, new Vector2(-18f, -18f), new Vector2(260f, 55f), new Vector2(1f, 1f));
        timerText.alignment = TextAlignmentOptions.TopRight;
        timerText.color = new Color(0.35f, 1f, 0.94f);

        startPanel = CreatePanel("StartPanel", root.transform, new Color(0.008f, 0.004f, 0.035f, 0.82f));
        CreateCenteredCard(startPanel.transform, true);

        resultPanel = CreatePanel("ResultPanel", root.transform, new Color(0.008f, 0.004f, 0.035f, 0.84f));
        CreateCenteredCard(resultPanel.transform, false);
        damageFlash.transform.SetAsLastSibling();
    }

    void CreateCenteredCard(Transform parent, bool isStart)
    {
        GameObject card = CreatePanel("Card", parent, new Color(0.035f, 0.02f, 0.085f, 0.97f));
        SetRect(card.GetComponent<RectTransform>(), Vector2.zero, new Vector2(530f, 350f), new Vector2(0.5f, 0.5f));

        Outline outline = card.AddComponent<Outline>();
        outline.effectColor = new Color(0.15f, 0.95f, 1f, 0.9f);
        outline.effectDistance = new Vector2(3f, -3f);

        if (isStart)
        {
            TextMeshProUGUI title = CreateText("Title", card.transform, "PORTAL // STATIC", 46f, FontStyles.Bold);
            SetRect(title.rectTransform, new Vector2(0f, 105f), new Vector2(470f, 70f), new Vector2(0.5f, 0.5f));
            title.alignment = TextAlignmentOptions.Center;
            title.color = new Color(0.35f, 1f, 0.94f);

            TextMeshProUGUI instructions = CreateText(
                "Instructions", card.transform,
                "A BODY MOVES BETWEEN SIGNALS\n\nDODGE THE RED FRAGMENTS FOR 30 SECONDS\nMOVE   W A S D   OR   ARROW KEYS\nTHREE HEARTS BEFORE THE PORTAL CLOSES",
                19f, FontStyles.Normal);
            SetRect(instructions.rectTransform, new Vector2(0f, 15f), new Vector2(460f, 155f), new Vector2(0.5f, 0.5f));
            instructions.alignment = TextAlignmentOptions.Center;
            instructions.color = new Color(0.9f, 0.84f, 1f);
            CreateButton("StartButton", card.transform, "ENTER", new Vector2(0f, -115f), StartGame);
        }
        else
        {
            resultTitle = CreateText("ResultTitle", card.transform, "GAME OVER", 46f, FontStyles.Bold);
            SetRect(resultTitle.rectTransform, new Vector2(0f, 85f), new Vector2(470f, 75f), new Vector2(0.5f, 0.5f));
            resultTitle.alignment = TextAlignmentOptions.Center;

            resultSubtitle = CreateText("ResultSubtitle", card.transform, string.Empty, 21f, FontStyles.Normal);
            SetRect(resultSubtitle.rectTransform, new Vector2(0f, 18f), new Vector2(460f, 70f), new Vector2(0.5f, 0.5f));
            resultSubtitle.alignment = TextAlignmentOptions.Center;
            resultSubtitle.color = new Color(0.88f, 0.93f, 1f);
            CreateButton("RestartButton", card.transform, "RETURN", new Vector2(0f, -90f), StartGame);
        }
    }

    Button CreateButton(string name, Transform parent, string label, Vector2 position, UnityEngine.Events.UnityAction action)
    {
        GameObject buttonObject = CreatePanel(name, parent, new Color(0.95f, 0.18f, 0.68f, 1f));
        SetRect(buttonObject.GetComponent<RectTransform>(), position, new Vector2(220f, 62f), new Vector2(0.5f, 0.5f));

        Button button = buttonObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.95f, 0.18f, 0.68f, 1f);
        colors.highlightedColor = new Color(0.35f, 1f, 0.94f, 1f);
        colors.pressedColor = new Color(0.7f, 0.08f, 0.5f, 1f);
        button.colors = colors;
        button.onClick.AddListener(action);

        TextMeshProUGUI buttonText = CreateText("Label", buttonObject.transform, label, 23f, FontStyles.Bold);
        Stretch(buttonText.rectTransform);
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = new Color(0.025f, 0.008f, 0.045f);
        buttonText.raycastTarget = false;
        return button;
    }

    TextMeshProUGUI CreateText(string name, Transform parent, string content, float size, FontStyles style)
    {
        GameObject textObject = CreateUiObject(name, parent);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = size;
        text.fontStyle = style;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.raycastTarget = false;
        return text;
    }

    GameObject CreatePanel(string name, Transform parent, Color color)
    {
        GameObject panel = CreateUiObject(name, parent);
        Stretch(panel.GetComponent<RectTransform>());
        Image image = panel.AddComponent<Image>();
        image.color = color;
        return panel;
    }

    GameObject CreateUiObject(string name, Transform parent)
    {
        GameObject uiObject = new GameObject(name, typeof(RectTransform));
        uiObject.layer = 5;
        uiObject.transform.SetParent(parent, false);
        return uiObject;
    }

    void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    void SetRect(RectTransform rect, Vector2 position, Vector2 size, Vector2 anchor)
    {
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    void CreateLiminalWorld()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        GameObject world = new GameObject("LiminalWorld");
        float height = mainCamera.orthographicSize * 2f;
        float width = height * mainCamera.aspect;
        Vector3 center = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 4f);

        GameObject background = new GameObject("DreamGradient");
        background.transform.SetParent(world.transform);
        background.transform.position = center;
        SpriteRenderer backgroundRenderer = background.AddComponent<SpriteRenderer>();
        backgroundRenderer.sprite = CreateGradientSprite();
        backgroundRenderer.sortingOrder = -1000;
        background.transform.localScale = new Vector3(
            width / backgroundRenderer.sprite.bounds.size.x,
            height / backgroundRenderer.sprite.bounds.size.y,
            1f);

        Sprite ringSprite = CreatePortalRingSprite();
        portalRings = new Transform[3];
        Vector2[] positions =
        {
            new Vector2(0.18f, 0.53f),
            new Vector2(0.50f, 0.48f),
            new Vector2(0.82f, 0.56f)
        };
        float[] sizes = { 3.3f, 5.1f, 3.8f };

        for (int i = 0; i < portalRings.Length; i++)
        {
            GameObject ring = new GameObject("SignalRing_" + (i + 1));
            ring.transform.SetParent(world.transform);
            Vector3 position = mainCamera.ViewportToWorldPoint(new Vector3(positions[i].x, positions[i].y, 0f));
            position.z = 3f;
            ring.transform.position = position;
            ring.transform.localScale = Vector3.one * sizes[i];
            SpriteRenderer renderer = ring.AddComponent<SpriteRenderer>();
            renderer.sprite = ringSprite;
            renderer.sortingOrder = -900 + i;
            renderer.color = i == 1
                ? new Color(1f, 0.3f, 0.8f, 0.62f)
                : new Color(0.2f, 0.95f, 1f, 0.5f);
            portalRings[i] = ring.transform;
        }

        Sprite fragmentSprite = CreateFragmentSprite();
        const int fragmentCount = 34;
        driftingFragments = new SpriteRenderer[fragmentCount];
        fragmentOrigins = new Vector3[fragmentCount];
        fragmentPhases = new float[fragmentCount];

        Random.State previousState = Random.state;
        Random.InitState(1707);
        for (int i = 0; i < fragmentCount; i++)
        {
            GameObject fragment = new GameObject("MemoryFragment_" + (i + 1));
            fragment.transform.SetParent(world.transform);
            Vector3 position = mainCamera.ViewportToWorldPoint(new Vector3(
                Random.Range(0.02f, 0.98f),
                Random.Range(0.04f, 0.96f),
                0f));
            position.z = 2f;
            fragment.transform.position = position;
            fragment.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            float size = Random.Range(0.025f, 0.085f);
            fragment.transform.localScale = new Vector3(size, size, 1f);

            SpriteRenderer renderer = fragment.AddComponent<SpriteRenderer>();
            renderer.sprite = fragmentSprite;
            renderer.sortingOrder = -800;
            renderer.color = i % 3 == 0
                ? new Color(1f, 0.25f, 0.75f, Random.Range(0.25f, 0.68f))
                : new Color(0.35f, 1f, 0.95f, Random.Range(0.2f, 0.58f));
            driftingFragments[i] = renderer;
            fragmentOrigins[i] = position;
            fragmentPhases[i] = Random.Range(0f, Mathf.PI * 2f);
        }
        Random.state = previousState;
    }

    void AnimateLiminalWorld()
    {
        if (portalRings != null)
        {
            for (int i = 0; i < portalRings.Length; i++)
            {
                Transform ring = portalRings[i];
                if (ring == null)
                    continue;

                float direction = i % 2 == 0 ? 1f : -1f;
                ring.Rotate(0f, 0f, direction * (5f + i * 2f) * Time.deltaTime);
                float pulse = 1f + Mathf.Sin(Time.time * (0.65f + i * 0.18f) + i) * 0.045f;
                float baseSize = i == 0 ? 3.3f : (i == 1 ? 5.1f : 3.8f);
                ring.localScale = Vector3.one * baseSize * pulse;
            }
        }

        if (driftingFragments == null)
            return;

        for (int i = 0; i < driftingFragments.Length; i++)
        {
            SpriteRenderer fragment = driftingFragments[i];
            if (fragment == null)
                continue;

            float phase = fragmentPhases[i];
            Vector3 offset = new Vector3(
                Mathf.Sin(Time.time * 0.23f + phase) * 0.12f,
                Mathf.Cos(Time.time * 0.31f + phase) * 0.18f,
                0f);
            fragment.transform.position = fragmentOrigins[i] + offset;
            fragment.transform.Rotate(0f, 0f, (i % 2 == 0 ? 8f : -8f) * Time.deltaTime);
        }
    }

    Sprite CreateGradientSprite()
    {
        const int textureWidth = 8;
        const int textureHeight = 256;
        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        texture.name = "LiminalGradient";
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;

        Color bottom = new Color(0.035f, 0.008f, 0.09f, 1f);
        Color middle = new Color(0.018f, 0.055f, 0.12f, 1f);
        Color top = new Color(0.025f, 0.008f, 0.075f, 1f);
        for (int y = 0; y < textureHeight; y++)
        {
            float t = y / (textureHeight - 1f);
            Color color = t < 0.52f
                ? Color.Lerp(bottom, middle, t / 0.52f)
                : Color.Lerp(middle, top, (t - 0.52f) / 0.48f);
            float horizon = Mathf.Exp(-Mathf.Pow((t - 0.46f) * 13f, 2f));
            color += new Color(0.035f, 0.008f, 0.08f, 0f) * horizon;
            for (int x = 0; x < textureWidth; x++)
                texture.SetPixel(x, y, color);
        }
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, textureWidth, textureHeight), new Vector2(0.5f, 0.5f), 100f);
    }

    Sprite CreatePortalRingSprite()
    {
        const int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.name = "ProceduralPortalRing";
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float nx = (x / (size - 1f)) * 2f - 1f;
                float ny = (y / (size - 1f)) * 2f - 1f;
                float radius = Mathf.Sqrt(nx * nx + ny * ny);
                float angle = Mathf.Atan2(ny, nx);
                float core = Mathf.Exp(-Mathf.Pow((radius - 0.68f) * 48f, 2f));
                float halo = Mathf.Exp(-Mathf.Pow((radius - 0.68f) * 10f, 2f)) * 0.2f;
                float glyphs = 0.52f + 0.48f * Mathf.Sin(angle * 18f + radius * 31f);
                float alpha = Mathf.Clamp01((core * (0.52f + glyphs * 0.48f)) + halo);
                Color cyan = new Color(0.08f, 1f, 0.95f, alpha);
                Color magenta = new Color(1f, 0.08f, 0.7f, alpha);
                Color color = Color.Lerp(cyan, magenta, (Mathf.Sin(angle * 2f) + 1f) * 0.5f);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    Sprite CreateFragmentSprite()
    {
        Texture2D texture = new Texture2D(8, 8, TextureFormat.RGBA32, false);
        texture.name = "SignalFragment";
        texture.filterMode = FilterMode.Bilinear;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                float distance = Mathf.Abs(x - 3.5f) + Mathf.Abs(y - 3.5f);
                float alpha = Mathf.Clamp01(1.35f - distance * 0.28f);
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, 8f, 8f), new Vector2(0.5f, 0.5f), 8f);
    }

    AudioClip CreateAmbientLoop()
    {
        const int sampleRate = 44100;
        const float duration = 2f;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float time = i / (float)sampleRate;
            float slowPulse = 0.55f + 0.45f * Mathf.Sin(2f * Mathf.PI * 0.5f * time);
            samples[i] = (
                Mathf.Sin(2f * Mathf.PI * 70f * time) * 0.13f +
                Mathf.Sin(2f * Mathf.PI * 117f * time) * 0.055f) * slowPulse;
        }

        AudioClip clip = AudioClip.Create("LiminalAmbient", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    AudioClip CreateTone(float frequency, float duration, float volume)
    {
        const int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float time = i / (float)sampleRate;
            float fade = 1f - (i / (float)sampleCount);
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * volume * fade;
        }

        AudioClip clip = AudioClip.Create("Tone" + frequency, sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
