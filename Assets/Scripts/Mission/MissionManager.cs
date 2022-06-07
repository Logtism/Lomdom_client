using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RiptideNetworking;

[System.Serializable]
public class MissionStart : UnityEvent { }
[System.Serializable]
public class MissionEnd : UnityEvent { }

public class MissionManager : MonoBehaviour
{
    private static MissionManager _singleton;
    public static MissionManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(MissionManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private bool MissionActive;

    public void StartMission(int mission_id)
    {
        Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.start_mission);
        message.AddInt(mission_id);
        NetworkManager.Singleton.Client.Send(message);

    }

    [MessageHandler((ushort)Messages.STC.mission_started)]
    private static void MissionStarted(Message message)
    {

    }

    [MessageHandler((ushort)Messages.STC.mission_already_active)]
    private static void MissionAlreadyActive(Message message)
    {

    }
}
