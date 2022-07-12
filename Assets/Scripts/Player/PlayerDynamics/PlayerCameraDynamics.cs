using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraDynamics : MonoBehaviour
{
    private static PlayerCameraDynamics _singleton;
    public static PlayerCameraDynamics Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(PlayerCameraDynamics)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [Header("General settings")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Vector3 defaultCameraPos;
    [SerializeField] private Quaternion defaultCameraRotation;
    [SerializeField] public bool reduceDynamics;

    [Header("Headbob settings")]
    [SerializeField] private bool headbobEnabled;
    [SerializeField] private float headbobTimer;
    [SerializeField, Range(0, 100)] private float headbobSpeed;
    private float returnSpeed = 100;
    [SerializeField, Range(0, 10)] private float headbobAmount;

    private void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) //Replace with better movement detection
        {
            if (headbobEnabled == true)
                doHeadbob();
        }
        else if(headbobTimer > 0)
        {
            headbobTimer = 0;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, defaultCameraPos, returnSpeed * Time.deltaTime);
        }
    }

    private void doHeadbob()
    {
        headbobTimer += Time.deltaTime * headbobSpeed;
        playerCamera.transform.localPosition = new Vector3(
            playerCamera.transform.localPosition.x,
            defaultCameraPos.y + Mathf.Sin(headbobTimer) * headbobAmount,
            playerCamera.transform.localPosition.z);
    }
}