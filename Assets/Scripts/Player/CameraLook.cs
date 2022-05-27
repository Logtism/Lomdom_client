using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float sensitvity = 3f;
    [SerializeField] private float ClampAngle = 85f;

    private float VertialRotation;
    private float HorizontalRotation;

    public GameObject cameraHolder;

    private void Start()
    {
        VertialRotation = transform.localEulerAngles.x;
        HorizontalRotation = player.transform.eulerAngles.y;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorMode();
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            //Look();
            LookButItsBetterCode();
        }

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);
    }

    private void LookButItsBetterCode()
    {
        transform.Rotate(Vector3.up* Input.GetAxisRaw("Mouse X") * sensitvity);

        ClampAngle += Input.GetAxisRaw("Mouse Y") * sensitvity;
        ClampAngle = Mathf.Clamp(ClampAngle, -90f, 90f);

		cameraHolder.transform.localEulerAngles = Vector3.left* ClampAngle;
}


    private void Look()
    {
        float MouseVertical = -Input.GetAxis("Mouse Y");
        float MouseHorizontal = Input.GetAxis("Mouse X");

        VertialRotation += MouseVertical * sensitvity * Time.deltaTime;
        HorizontalRotation += MouseHorizontal * sensitvity * Time.deltaTime;

        VertialRotation = Mathf.Clamp(VertialRotation, -ClampAngle, ClampAngle);

        transform.localRotation = Quaternion.Euler(VertialRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, HorizontalRotation, 0f);
    }

    private void ToggleCursorMode()
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
