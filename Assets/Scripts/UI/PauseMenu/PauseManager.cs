using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _singleton;
    public static PauseManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(PauseManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private GameObject mainMenuPanel;
    public bool menuIsOpen = false;
    public bool canOpenMenu = true;

    private void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && MissionMenuManager.Singleton.missionMenuOpen == false && ChatUI.Singleton.ChatOpen == false)
        {
            if(menuIsOpen) { manualMenuClose(); }

            else { manualMenuOpen(); }
        }
    }

    public void manualMenuClose()
    {
        mainMenuPanel.SetActive(false);

        HUDmanager.Singleton.showFullUI();
        PlayerMove.Singleton.canMove = true;
        CameraLook.Singleton.ToggleCursorMode();

        menuIsOpen = false;
    }

    public void manualMenuOpen()
    {
        if (canOpenMenu)
        {
            mainMenuPanel.SetActive(true);

            PlayerMove.Singleton.canMove = false;
            CameraLook.Singleton.ToggleCursorMode();

            if (HUDmanager.Singleton.mapUIactive || HUDmanager.Singleton.ammoUIactive || HUDmanager.Singleton.healthUIactive || HUDmanager.Singleton.moneyUIactive)
            {
                HUDmanager.Singleton.forceHideHUD();
            }

            if(WeaponWheel.Singleton.WheelActive == true)
            {
                WeaponWheel.Singleton.ToggleWheel();
            }

            menuIsOpen = true;
        }
    }
}
