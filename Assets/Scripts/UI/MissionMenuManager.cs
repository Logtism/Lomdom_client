using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject missionMenu;
    [SerializeField] private Mission selectedMissionOnStart;
    [SerializeField] private TextMeshProUGUI missionTypeText;
    [SerializeField] private TextMeshProUGUI missionRewardText;
    [SerializeField] private TextMeshProUGUI otherMissionInfoText;   
    
    public bool missionMenuOpen;
    public bool canStartMission;

    private void Awake()
    {
        missionMenuOpen = false;
        missionMenu.SetActive(false);

        missionTypeText.text = selectedMissionOnStart.MissionName.ToString();
        //missionRewardText.text = "Mission reward: $" + selectedMissionOnStart.MissionReward.ToString();
        //otherMissionInfoText.text = selectedMissionOnStart.MissionInfo.ToString();
    }


    public void openMissionMenu()
    {
        missionMenu.SetActive(true);
        missionMenuOpen = true;

        GameObject LocalPlayer = GameObject.FindWithTag("LocalPlayer");
        LocalPlayer.GetComponent<PlayerMove>().canMove = false;
    }

    public void closeMissionMenu()
    {
        missionMenu.SetActive(false);
        missionMenuOpen = false;

        GameObject LocalPlayer = GameObject.FindWithTag("LocalPlayer");
        LocalPlayer.GetComponent<PlayerMove>().canMove = true;
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
            //otherMissionInfoText.text = mission.MissionInfo.ToString();

            selectedMissionOnStart = mission;

            //MapShit
        }
    }

    public void startMission()
    {
        if(missionMenuOpen && canStartMission)
        {
            //start mission logic using selectedMissionOnStart
        }
        
        else
        {
            Debug.Log("Player is not allowed to start mission");
        }
    }
}
