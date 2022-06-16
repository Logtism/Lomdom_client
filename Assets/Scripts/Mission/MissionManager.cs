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

    [SerializeField] private Mission[] missions;
    private Mission ActiveMission;

    public void StartMission(int mission_id)
    {
        Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.start_mission);
        message.AddInt(mission_id);
        NetworkManager.Singleton.Client.Send(message);

    }

    [MessageHandler((ushort)Messages.STC.mission_started)]
    private static void MissionStarted(Message message)
    {
        Singleton.ActiveMission = Singleton.missions[message.GetInt()];
        Singleton.ActiveMission.MissionStartFunction.Invoke();
    }

    [MessageHandler((ushort)Messages.STC.mission_ended)]
    private static void MissionEnded(Message message)
    {
        Singleton.ActiveMission.MissionEndFunction.Invoke();
        Singleton.ActiveMission = null;
    }

    [MessageHandler((ushort)Messages.STC.mission_already_active)]
    private static void MissionAlreadyActive(Message message)
    {

    }
}
