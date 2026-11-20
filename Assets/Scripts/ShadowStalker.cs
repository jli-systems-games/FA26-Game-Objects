using System.Collections;
using UnityEngine;
using TMPro;

public class ShadowStalker : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public CanvasGroup fadeOverlay;
    public TMP_Text whisperText;
    public CanvasGroup whisperGroup;

    [Header("走廊：追逐")]
    public float chaseSpeed = 2.2f;
    public float catchDistance = 1.2f;
    public float cooldownAfterCatch = 8f;   // 被抓后它呆住的秒数

    [Header("展厅：游荡")]
    public float wanderSpeed = 1.0f;
    public float wanderPauseMin = 2f;
    public float wanderPauseMax = 5f;
    public float keepAwayFromPlayer = 3f;

    [Header("低语")]
    public float whisperIntervalMin = 10f;
    public float whisperIntervalMax = 20f;
    public float whisperHoldTime = 4f;
    public float whisperRange = 11f;

    [Header("被抓时的黑屏")]
    public float fadeInTime = 0.5f;
    public float deathHoldTime = 2.5f;      // 玩家读那两行字的时间
    public float fadeOutTime = 1.0f;

    [Header("General")]
    public float hoverHeight = 1.2f;
    public float retreatDistance = 16f;

    [Header("Audio")]
    public AudioSource proximityLoop;
    public float audibleRange = 12f;

    private bool isCatching = false;
    private float passiveUntil = 0f;
    private Vector3 wanderTarget;
    private float wanderPauseUntil = 0f;
    private float nextWhisperTime = 0f;
    private SafeZone lastZone;

    void Awake()
    {
        SafeZone.SafeZoneCount = 0;
        SafeZone.CurrentZone = null;
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        SafeZone.LastSafePoint = (player != null) ? player.position : transform.position;

        if (fadeOverlay != null) fadeOverlay.alpha = 0f;
        if (whisperGroup != null) whisperGroup.alpha = 0f;
        if (proximityLoop != null) proximityLoop.volume = 0f;

        wanderTarget = transform.position;
        nextWhisperTime = Time.time + Random.Range(whisperIntervalMin, whisperIntervalMax);
    }

    void Update()
    {
        if (player == null || isCatching) return;

        SafeZone zone = SafeZone.CurrentZone;
        bool safe = SafeZone.SafeZoneCount > 0;
        bool passive = Time.time < passiveUntil;

        if (safe && zone != null && zone.shadowMayEnter)
            Wander(zone);
        else if (safe)
            KeepDistance();
        else if (!passive)
            Chase();
        // passive 且在走廊 → 原地不动，给玩家一段逃跑时间

        UpdateSound();
        TryWhisper(safe, zone);
    }

    void Chase()
    {
        Vector3 target = new Vector3(player.position.x, hoverHeight, player.position.z);
        MoveTo(target, chaseSpeed);
        FaceTowards(target);

        if (Vector3.Distance(transform.position, target) < catchDistance)
            StartCoroutine(CatchPlayer());
    }

    void Wander(SafeZone zone)
    {
        // 换了房间 → 立刻在新房间里重选目标（这是它之前不进展厅的原因）
        if (zone != lastZone)
        {
            lastZone = zone;
            wanderTarget = zone.RandomPointInside();
            wanderPauseUntil = 0f;
        }

        if (Time.time < wanderPauseUntil)
        {
            FaceTowards(player.position);   // 停下来看着你
            return;
        }

        Vector3 flat = new Vector3(wanderTarget.x, hoverHeight, wanderTarget.z);
        float dist = Vector3.Distance(transform.position, flat);

        if (dist < 0.5f)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 p = zone.RandomPointInside();
                if (Vector3.Distance(p, player.position) > keepAwayFromPlayer)
                {
                    wanderTarget = p;
                    break;
                }
            }
            wanderPauseUntil = Time.time + Random.Range(wanderPauseMin, wanderPauseMax);
            return;
        }

        // 还在很远（比如刚从走廊往展厅飘）就走快一点，到了附近再慢下来
        float speed = (dist > 6f) ? wanderSpeed * 2.5f : wanderSpeed;
        MoveTo(flat, speed);
        FaceTowards(flat);
    }

    void KeepDistance()
    {
        Vector3 away = transform.position - player.position;
        away.y = 0f;
        if (away.sqrMagnitude < 0.01f) away = Vector3.forward;

        if (away.magnitude < retreatDistance)
        {
            Vector3 target = player.position + away.normalized * retreatDistance;
            target.y = hoverHeight;
            MoveTo(target, 5f);
        }
    }

    void MoveTo(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void FaceTowards(Vector3 point)
    {
        Vector3 look = point - transform.position;
        look.y = 0f;
        if (look.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(
                transform.rotation, Quaternion.LookRotation(look), 2f * Time.deltaTime);
    }

    void UpdateSound()
    {
        if (proximityLoop == null) return;
        float d = Vector3.Distance(transform.position, player.position);
        float t = Mathf.Clamp01(1f - d / audibleRange);
        proximityLoop.volume = t * t;
    }

    void TryWhisper(bool safe, SafeZone zone)
    {
        if (whisperText == null || whisperGroup == null) return;
        if (!safe || zone == null) return;
        if (Time.time < nextWhisperTime) return;
        if (Vector3.Distance(transform.position, player.position) > whisperRange) return;

        string line = zone.RandomLine();
        if (!string.IsNullOrEmpty(line)) StartCoroutine(ShowWhisper(line));

        nextWhisperTime = Time.time + Random.Range(whisperIntervalMin, whisperIntervalMax);
    }

    IEnumerator ShowWhisper(string line)
    {
        whisperText.text = line;
        yield return FadeGroup(whisperGroup, 1f, 1.2f);
        yield return new WaitForSeconds(whisperHoldTime);
        yield return FadeGroup(whisperGroup, 0f, 2f);
    }

    IEnumerator CatchPlayer()
    {
        isCatching = true;

        PlayerController pc = player.GetComponent<PlayerController>();
        PlayerInteraction pi = player.GetComponent<PlayerInteraction>();
        CharacterController cc = player.GetComponent<CharacterController>();

        // 正在读说明牌就强制合上——记忆被读到一半抽走
        if (pi != null) pi.ForceClosePanel();

        // 黑屏期间彻底冻住玩家：不能走、不能再开面板
        if (pc != null) pc.enabled = false;
        if (pi != null) pi.enabled = false;

        // 低语收掉，别和死亡文字抢
        if (whisperGroup != null) whisperGroup.alpha = 0f;

        yield return FadeGroup(fadeOverlay, 1f, fadeInTime);
        yield return new WaitForSeconds(deathHoldTime);

        // 传送：必须先关 CharacterController，否则位置会被拽回去
        if (cc != null) cc.enabled = false;
        player.position = SafeZone.LastSafePoint;
        if (cc != null) cc.enabled = true;

        // 黑影退远
        Vector3 away = transform.position - player.position;
        away.y = 0f;
        if (away.sqrMagnitude < 0.01f) away = Vector3.forward;
        transform.position = player.position + away.normalized * retreatDistance
                           + Vector3.up * hoverHeight;

        passiveUntil = Time.time + cooldownAfterCatch;
        lastZone = null;

        yield return FadeGroup(fadeOverlay, 0f, fadeOutTime);

        // 恢复控制，锁回鼠标
        if (pc != null) pc.enabled = true;
        if (pi != null) pi.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isCatching = false;
    }

    IEnumerator FadeGroup(CanvasGroup g, float target, float duration)
    {
        if (g == null) yield break;
        float start = g.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            g.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        g.alpha = target;
    }
}