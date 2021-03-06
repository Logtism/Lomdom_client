using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RiptideNetworking;
using UnityEngine.UI;

public class MissionMenuManager : MonoBehaviour
{
    private static MissionMenuManager _singleton;
    public static MissionMenuManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(MissionMenuManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private GameObject missionMenu;
    [SerializeField] private Mission selectedMissionOnStart;
    [SerializeField] private TextMeshProUGUI missionTypeText;
    [SerializeField] private TextMeshProUGUI missionRewardText;
    [SerializeField] private GameObject[] missionMenuTabs;
    
    public bool missionMenuOpen;
    public bool canStartMission;

    private void Awake()
    {
        Singleton = this;

        missionMenuOpen = false;
        missionMenu.SetActive(false);

        missionTypeText.text = selectedMissionOnStart.MissionName.ToString();
        //missionRewardText.text = "Mission reward: $" + selectedMissionOnStart.MissionReward.ToString();
    }


    public void openMissionMenu()
    {
        missionMenu.SetActive(true);
        missionMenuOpen = true;

        GameObject LocalPlayer = GameObject.FindWithTag("LocalPlayer");
        LocalPlayer.GetComponent<PlayerMove>().canMove = false;

        CameraLook.Singleton.ToggleCursorMode();
        HUDmanager.Singleton.toggleCrosshair();
    }

    public void switchActiveMissionMenu(GameObject newTab)
    {
        newTab.SetActive(true);

        for (int i = 0; i < missionMenuTabs.Length; i++)
        {
            if(missionMenuTabs[i] != newTab)
            {
                missionMenuTabs[i].SetActive(false);
            }
        }
    }

    public void closeMissionMenu()
    {
        missionMenu.SetActive(false);

        StartCoroutine(closeDelay());

        GameObject LocalPlayer = GameObject.FindWithTag("LocalPlayer");
        LocalPlayer.GetComponent<PlayerMove>().canMove = true;

        CameraLook.Singleton.ToggleCursorMode();
        HUDmanager.Singleton.toggleCrosshair();
    }

    public void updateMissionInfo(Mission mission)
    {
        if(missionMenuOpen == false)
        {
            Debug.Log("mission menu closed. cannot update mission info. currently selected mission is " + selectedMissionOnStart);
        }
        
        else
        {
            missionTypeText.text = mission.MissionName.ToString();
            //missionRewardText.text = "Mission reward: $" + mission.MissionReward.ToString();

            selectedMissionOnStart = mission;

            //MapShit
        }
    }

    public void startMission()
    {
        if(missionMenuOpen && canStartMission)
        {
            //start mission logic using selectedMissionOnStart
            MissionManager.Singleton.StartMission(selectedMissionOnStart.MissionId);
        }
        
        else
        {
            Debug.Log("Player is not allowed to start mission");
        }
    }

    private IEnumerator closeDelay()
    {
        yield return new WaitForSeconds(0.01f);
        missionMenuOpen = false;
    }
}
