using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class CTS_InteractionInputHandler : MonoBehaviour
{
    private static CTS_InteractionInputHandler _singleton;
    public static CTS_InteractionInputHandler Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(CTS_InteractionInputHandler)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private bool checkInputForRobbery;
    [SerializeField] private float robberyHoldTime;
    private float robberyTimer;

    private void Awake()
    {
        Singleton = this;

        robberyTimer = robberyHoldTime;
    }

    [MessageHandler((ushort)Messages.STC.playerEnter_RobberyTrigger)]
    private static void OnPlayerEnterRobberyTrigger(Message message)
    {
        Singleton.checkInputForRobbery = true;
    }

    [MessageHandler((ushort)Messages.STC.playerExit_RobberyTrigger)]
    private static void OnPlayerExitRobberyTrigger(Message message)
    {
        Singleton.checkInputForRobbery = false;
    }

    private void Update()
    {
        if (checkInputForRobbery)
        {
            if (Input.GetKey(KeyCode.E))
            {
                robberyTimer -= Time.deltaTime;
                Debug.Log(robberyTimer);
                if (robberyTimer < 0)
                {
                    Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.completeRobbery_input);
                    NetworkManager.Singleton.Client.Send(message);
                }
            }
            else
                robberyTimer = robberyHoldTime;
        }
    }
}
