using UnityEngine;
using UnityEngine.InputSystem;

public class BlueCubeBehaviour : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpHeight = 2.5f;
    public float jumpDuration = 0.9f;

    private bool isRedCube;
    private bool isBlueCube;
    private Transform redCube;
    private float groundY;
    private float jumpTime;
    private bool isJumping;
    private GUIStyle controlsStyle;

    void Start()
    {
        isRedCube = gameObject.name == "RedCube";
        isBlueCube = gameObject.name == "BlueCube";
        groundY = transform.position.y;

        if (isBlueCube)
        {
            GameObject player = GameObject.Find("RedCube");
            if (player != null)
                redCube = player.transform;
            else
                Debug.LogWarning("BlueCube could not find RedCube.", this);
        }
    }

    void Update()
    {
        if (!isRedCube)
            return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            // Move on world X/Z; normalize so diagonal movement is not faster.
            float x = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f)
                    - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
            float z = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f)
                    - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);
            transform.position += new Vector3(x, 0f, z).normalized * movementSpeed * Time.deltaTime;

            if (keyboard.spaceKey.wasPressedThisFrame && !isJumping)
            {
                isJumping = true;
                jumpTime = 0f;
            }
        }

        // Follow a smooth jump arc, ignore extra presses, and land at the exact starting Y.
        if (isJumping)
        {
            jumpTime += Time.deltaTime;
            float progress = Mathf.Clamp01(jumpTime / Mathf.Max(0.01f, jumpDuration));
            Vector3 position = transform.position;
            position.y = groundY + Mathf.Max(0f, jumpHeight) * Mathf.Sin(progress * Mathf.PI);
            if (progress >= 1f)
            {
                position.y = groundY;
                isJumping = false;
            }
            transform.position = position;
        }
    }

    void OnGUI()
    {
        // Draw once at the top, leaving the gameplay area clear.
        if (!isRedCube)
            return;

        if (controlsStyle == null)
        {
            controlsStyle = new GUIStyle(GUI.skin.box);
            controlsStyle.alignment = TextAnchor.MiddleCenter;
            controlsStyle.wordWrap = true;
            controlsStyle.normal.textColor = Color.white;
        }

        controlsStyle.fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height / 60f), 10, 16);
        string controls = "WASD / Arrow Keys: Move RedCube\nSpacebar: Jump\nQ: Teleport BlueCube to RedCube's position and rotation";
        float width = Mathf.Min(480f, Screen.width * 0.9f);
        float height = controlsStyle.CalcHeight(new GUIContent(controls), width) + 8f;
        GUI.Box(new Rect((Screen.width - width) / 2f, 8f,
            width, height), controls, controlsStyle);
    }

    void LateUpdate()
    {
        // Q copies the player's latest position and rotation after movement, preserving scale.
        Keyboard keyboard = Keyboard.current;
        if (isBlueCube && redCube != null && keyboard != null && keyboard.qKey.wasPressedThisFrame)
            transform.SetPositionAndRotation(redCube.position, redCube.rotation);
    }
}
