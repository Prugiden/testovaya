using UnityEngine;
using UnityEngine.UI;

public class MobileFPSController : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public RectTransform touchField;
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float minVerticalAngle = -60f;
    public float maxVerticalAngle = 60f;

    private Transform cameraTransform;
    private CharacterController controller;
    private Vector2 lookInput;
    private float verticalRotation = 0f;
    private bool isTouchingField = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Move();
        LookAround();
    }

    void Move()
    {
        Vector3 moveDirection = new Vector3(moveJoystick.Horizontal, 0f, moveJoystick.Vertical);
        moveDirection = transform.TransformDirection(moveDirection);
        controller.SimpleMove(moveDirection * moveSpeed);
    }

    void LookAround()
    {
        lookInput = Vector2.zero;
        isTouchingField = false;

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(touchField, touch.position))
                {
                    isTouchingField = true;
                    if (touch.phase == TouchPhase.Moved)
                    {
                        lookInput = touch.deltaPosition;
                    }
                }
            }
        }

        if (isTouchingField)
        {
            float horizontalRotation = lookInput.x * lookSpeed * Time.deltaTime;
            float verticalRotationDelta = -lookInput.y * lookSpeed * Time.deltaTime;

            verticalRotation = Mathf.Clamp(verticalRotation + verticalRotationDelta, minVerticalAngle, maxVerticalAngle);

            transform.Rotate(0f, horizontalRotation, 0f);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }
}
