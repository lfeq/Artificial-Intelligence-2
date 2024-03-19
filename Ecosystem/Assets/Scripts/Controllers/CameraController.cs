using UnityEngine;

/// <summary>
/// Controls the movement and rotation of the camera in the scene.
/// </summary>
public class CameraController : MonoBehaviour {
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_rotationSpeed = 100f;
    [SerializeField] private float m_ascendDescendSpeed = 3f;

    private float verticalRotation = 0f;

    private void Update() {
        if (LevelManager.s_instance.getLevelState() != LevelState.Playing) {
            return;
        }
        if (!Input.GetMouseButton(1)) {
            Cursor.visible = true;
            return;
        }
        // Hide Mouse
        Cursor.visible = false;
        // Translation
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(moveDirection * m_moveSpeed * Time.deltaTime);
        // Rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.up * mouseX * m_rotationSpeed * Time.deltaTime);
        verticalRotation -= mouseY * m_rotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localEulerAngles.y, 0f);
        // Ascend/Descend
        if (Input.GetKey(KeyCode.E)) {
            transform.Translate(Vector3.up * m_ascendDescendSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.Q)) {
            transform.Translate(Vector3.down * m_ascendDescendSpeed * Time.deltaTime);
        }
    }
}