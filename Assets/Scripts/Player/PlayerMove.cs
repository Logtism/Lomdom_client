using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;

public class PlayerMove : MonoBehaviour
{
    private static PlayerMove _singleton;
    public static PlayerMove Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(PlayerMove)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private Transform camTransform;

    private bool[] inputs;

    public bool canMove = true;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        inputs = new bool[6];
    }

    private void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                inputs[0] = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputs[1] = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputs[2] = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputs[3] = true;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                inputs[4] = true;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputs[5] = true;
            }
        }
    }

    private void FixedUpdate()
    {
        SendInput();

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = false;
        }
    }

    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.unreliable, Messages.CTS.inputs);
        message.AddBools(inputs, false);
        message.AddVector3(camTransform.forward);
        NetworkManager.Singleton.Client.Send(message);
    }
}
