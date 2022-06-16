using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    private bool[] inputs;

    public bool canMove = true;

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
