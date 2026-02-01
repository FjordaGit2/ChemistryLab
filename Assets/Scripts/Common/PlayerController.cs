using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    float xRotation = 0f;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float moveSpeed = 100f;
    [SerializeField] bool isMove = false;
    [SerializeField] bool isRigidbody = false;
    Transform cameraTransform;
    Rigidbody rb;
    CharacterController controller;
    private void Start()
    {
        cameraTransform = transform.GetComponentInChildren<Camera>().transform;
        if (isMove)
        {
            if (isRigidbody)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.drag = 2;
            }
            else
            {
                controller = gameObject.AddComponent<CharacterController>();
            }
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up, mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation , -90 , 90);
        cameraTransform.localRotation = Quaternion.Euler(xRotation , 0f , 0f);
        if (isMove && !LevelScript.PlayerFreeze)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            if (isRigidbody)
            {
                rb.MovePosition(transform.position + move * Time.deltaTime * moveSpeed);
            }
            else
            {
                controller.Move(move * Time.deltaTime * moveSpeed);
            }
        }
    }
}
