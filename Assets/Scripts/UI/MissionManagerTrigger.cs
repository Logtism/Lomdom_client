using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class MissionManagerTrigger : MonoBehaviour
{
    private static MissionManagerTrigger _singleton;
    public static MissionManagerTrigger Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(MissionManagerTrigger)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private bool playerInTrigger;

    [MessageHandler((ushort)Messages.STC.playerEnter_missionTrigger)]
    private static void OnPlayerEnterTrigger(Message message)
    {
        MissionMenuManager.Singleton.canStartMission = true;
        Singleton.playerInTrigger = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInTrigger && MissionMenuManager.Singleton.missionMenuOpen == false)
        {
            MissionMenuManager.Singleton.openMissionMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && playerInTrigger && MissionMenuManager.Singleton.missionMenuOpen == true)
        {
            MissionMenuManager.Singleton.closeMissionMenu();
        }
    }

    [MessageHandler((ushort)Messages.STC.playerExit_missionTrigger)]
    private static void OnPlayerExitTrigger(Message message)
    {
        MissionMenuManager.Singleton.canStartMission = false;
        Singleton.playerInTrigger = false;
    }
}
