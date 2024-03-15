using UnityEngine;

//TODO: Solo poder mover la camara cuando el levelmanager esta en estado playing
public class CameraController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float ascendDescendSpeed = 3f;

    private float verticalRotation = 0f;

    private void Update() {
        // Hide Mouse
        Cursor.visible = false;
        // Translation
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        // Rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
        verticalRotation -= mouseY * rotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localEulerAngles.y, 0f);
        // Ascend/Descend
        if (Input.GetKey(KeyCode.E)) {
            transform.Translate(Vector3.up * ascendDescendSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.Q)) {
            transform.Translate(Vector3.down * ascendDescendSpeed * Time.deltaTime);
        }
    }
}