using UnityEngine;
using UnityEngine.InputSystem;

public class characterlook : MonoBehaviour
{
    // Optional references. The script finds or creates these when they are not assigned.
    public GameObject Camera;
    public CharacterController control;
    public InputAction move;
    public InputAction look;
    public float speed = 3f;
    public float mouseSensitivity = .12f;
    public float gravity = -18f;

    private Transform body;
    private Transform view;
    private float yaw;
    private float pitch;
    private float verticalSpeed;
    private PlayerInput playerInput;
    private bool lookActive = true;

    private void Awake()
    {
        var explorer = GetComponent<GalleryExplorerController>();
        if (explorer != null) explorer.enabled = false;

        if (Camera == null)
        {
            var foundCamera = GetComponentInChildren<UnityEngine.Camera>();
            if (foundCamera != null) Camera = foundCamera.gameObject;
        }
        view = Camera != null ? Camera.transform : transform;

        // When this script is on the camera itself, give the camera a separate player body
        // so looking up and down does not tilt the CharacterController capsule.
        if (control != null)
        {
            body = control.transform;
        }
        else if (GetComponent<CharacterController>() != null && view != transform)
        {
            body = transform;
        }
        else if (view == transform)
        {
            var existingParent = transform.parent;
            if (existingParent != null && existingParent.GetComponent<CharacterController>() != null)
            {
                body = existingParent;
            }
            else
            {
                var playerRig = new GameObject("Player Controller");
                playerRig.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, transform.eulerAngles.y, 0));
                transform.SetParent(playerRig.transform, true);
                body = playerRig.transform;
            }
        }
        else
        {
            body = transform;
        }

        foreach (var duplicateExplorer in body.GetComponentsInChildren<GalleryExplorerController>(true))
            duplicateExplorer.enabled = false;

        // A dynamic Rigidbody or a second solid collider on the player fights the
        // CharacterController and causes the player to shake or get pushed through floors.
        var viewPosition = view.position;
        var viewRotation = view.rotation;
        var rigidbody = body.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            rigidbody.detectCollisions = false;
            Destroy(rigidbody);
        }
        foreach (var bodyCollider in body.GetComponents<Collider>())
            if (!(bodyCollider is CharacterController)) bodyCollider.enabled = false;

        // CharacterController does not behave reliably on a scaled player object.
        body.localScale = Vector3.one;
        if (view != body) view.SetPositionAndRotation(viewPosition, viewRotation);

        control = body.GetComponent<CharacterController>();
        if (control == null) control = body.gameObject.AddComponent<CharacterController>();
        control.height = 1.7f;
        control.radius = .28f;
        control.center = new Vector3(0, .85f - body.position.y, 0);
        control.skinWidth = .08f;
        control.stepOffset = .25f;
        control.slopeLimit = 45f;
        control.minMoveDistance = 0f;
        control.enableOverlapRecovery = true;

        yaw = body.eulerAngles.y;
        pitch = view == body ? 0 : view.localEulerAngles.x;
        if (pitch > 180) pitch -= 360;

        playerInput = body.GetComponent<PlayerInput>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (playerInput != null && playerInput.actions != null)
        {
            if (move == null) move = playerInput.actions.FindAction("Player/Move", false);
            if (look == null) look = playerInput.actions.FindAction("Player/Look", false);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            lookActive = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Cursor.lockState != CursorLockMode.Locked && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            lookActive = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector2 moveInput = Vector2.zero;
        if (move != null && move.enabled)
            moveInput = move.ReadValue<Vector2>();
        // A serialized InputAction can exist without any bindings. Fall back to
        // direct keyboard input whenever it is disabled or currently returns zero.
        if (moveInput.sqrMagnitude < .0001f)
            moveInput = ReadKeyboardMove();
        Vector3 direction = body.forward * moveInput.y + body.right * moveInput.x;
        direction.y = 0;
        if (direction.sqrMagnitude > 1) direction.Normalize();

        if (control.isGrounded && verticalSpeed < 0) verticalSpeed = -2f;
        else verticalSpeed += gravity * Time.deltaTime;
        Vector3 velocity = direction * speed + Vector3.up * verticalSpeed;
        var collisionFlags = control.Move(velocity * Time.deltaTime);
        if ((collisionFlags & CollisionFlags.Below) != 0 && verticalSpeed < 0) verticalSpeed = -2f;

        if (lookActive)
        {
            Vector2 mouseDelta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
            if (look != null && look.enabled)
            {
                Vector2 actionDelta = look.ReadValue<Vector2>();
                if (actionDelta.sqrMagnitude > 0) mouseDelta = actionDelta;
            }
            yaw += mouseDelta.x * mouseSensitivity;
            pitch = Mathf.Clamp(pitch - mouseDelta.y * mouseSensitivity, -78f, 78f);
            body.rotation = Quaternion.Euler(0, yaw, 0);
            if (view != body) view.localRotation = Quaternion.Euler(pitch, 0, 0);
            else body.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }

    private Vector2 ReadKeyboardMove()
    {
        if (Keyboard.current == null) return Vector2.zero;
        float x = (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1 : 0)
                - (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1 : 0);
        float y = (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1 : 0)
                - (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1 : 0);
        return new Vector2(x, y).normalized;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) return;
        lookActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
