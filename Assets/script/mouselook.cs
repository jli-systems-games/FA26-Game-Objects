using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("鼠标灵敏度")]
    public float mouseSensitivity = 100f;

    [Header("上下视角限制")]
    public float minPitch = -90f;
    public float maxPitch = 90f;

    float yaw;   // 左右旋转
    float pitch; // 上下抬头低头

    void Start()
    {
        // 进游戏直接锁住鼠标，隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 读取初始旋转，避免开局跳一下
        Vector3 rot = transform.localEulerAngles;
        yaw = rot.y;
        pitch = rot.x;
    }

    void Update()
    {
        // 获取鼠标移动增量
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        // 夹住上下角度，防止倒转
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}