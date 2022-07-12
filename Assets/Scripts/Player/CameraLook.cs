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

    private float VertialRotation;
    private float HorizontalRotation;

    private bool cursorActive;

    public GameObject cameraHolder;
    private void Awake()
    {
        Singleton = this;
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
        if (cursorActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cursorActive = false;
            return;
        }

        if(cursorActive == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            cursorActive = true;
        }
    }
}
