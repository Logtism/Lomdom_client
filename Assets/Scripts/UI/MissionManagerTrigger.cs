using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManagerTrigger : MonoBehaviour
{
    private bool playerInTrigger;
    private GameObject MissionMenuManager;

    private void Awake()
    {
        MissionMenuManager = GameObject.FindWithTag("MissionMenuManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LocalPlayer")
        {
            MissionMenuManager.GetComponent<MissionMenuManager>().canStartMission = true;

            playerInTrigger = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInTrigger && MissionMenuManager.GetComponent<MissionMenuManager>().missionMenuOpen == false)
        {
            MissionMenuManager.GetComponent<MissionMenuManager>().openMissionMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && playerInTrigger && MissionMenuManager.GetComponent<MissionMenuManager>().missionMenuOpen == true)
        {
            MissionMenuManager.GetComponent<MissionMenuManager>().closeMissionMenu();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LocalPlayer")
        {
            MissionMenuManager.GetComponent<MissionMenuManager>().canStartMission = false;

            playerInTrigger = false;
        }
    }
}
