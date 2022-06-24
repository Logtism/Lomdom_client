using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private static CameraLook _singleton;
    public static CameraLook Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(CameraLook)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [Header("Camera look settings")]
    [SerializeField] private Player player;
    [SerializeField] private float sensitvity = 3f;
    [SerializeField] private float ClampAngle = 85f;

    [Header("Headbob settings")]
    [SerializeField] private bool headbobEnabled;
    [SerializeField, Range(0, 0.1f)] private float headbobAmplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float headbobFreqeuncy = 10.0f;
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform playerCameraHolder = null;
    private float toggleSpeed = 3.0f;
    private Vector3 startPos;

    private float VertialRotation;
    private float HorizontalRotation;

    public GameObject cameraHolder;
    private void Awake()
    {
        Singleton = this;

        startPos = playerCamera.localPosition;
    }

    private void Start()
    {
        VertialRotation = transform.localEulerAngles.x;
        HorizontalRotation = player.transform.eulerAngles.y;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);

        if (!headbobEnabled) return;

        CheckMotion();
        ResetPosition();
    }

    private void CheckMotion()
    {
        if (Input.GetKey(KeyCode.W)) PlayMotion(footstepMotion());
        if (Input.GetKey(KeyCode.A)) PlayMotion(footstepMotion());
        if (Input.GetKey(KeyCode.S)) PlayMotion(footstepMotion());
        if (Input.GetKey(KeyCode.D)) PlayMotion(footstepMotion());

        else return;
    }

    private void PlayMotion(Vector3 motion)
    {
        playerCamera.localPosition += motion;
    }

    private Vector3 footstepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * headbobFreqeuncy) * headbobAmplitude;
        pos.x += Mathf.Cos(Time.time * headbobFreqeuncy / 2) * headbobAmplitude * 2;
        return pos;
    }

    private void ResetPosition()
    {
        if (playerCamera.localPosition == startPos) return;
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, startPos, 1 * Time.deltaTime);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up* Input.GetAxisRaw("Mouse X") * sensitvity);

        ClampAngle += Input.GetAxisRaw("Mouse Y") * sensitvity;
        ClampAngle = Mathf.Clamp(ClampAngle, -90f, 90f);

		cameraHolder.transform.localEulerAngles = Vector3.left* ClampAngle;
    }

    public void ToggleCursorMode()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
