using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject PauseMenuDefault;
    [SerializeField] GameObject SettingsMenu;

    [Header("Conformation windows")]
    [SerializeField] GameObject QuitToTitleConformation;
    [SerializeField] GameObject ExitToDesktopConformation;

    [Header("Other")]
    [SerializeField] GameObject HUD_manager;
    public bool pauseMenuOpen;


    private void Awake()
    {
        pauseMenuOpen = false;

        PauseMenuDefault.SetActive(false);
        SettingsMenu.SetActive(false);
        QuitToTitleConformation.SetActive(false);
        ExitToDesktopConformation.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenuOpen)
            {
                pauseMenuOpen = true;
                HUD_manager.GetComponent<HUDmanager>().forceHideHUD();
                HUD_manager.GetComponent<HUDmanager>().hudEnabled = false;

                toPauseDefault();
            }

            else
            {
                closePauseMenu();
            }
        }
    }

    public void closePauseMenu()
    {
        pauseMenuOpen = false;
        
        PauseMenuDefault.SetActive(false);
        SettingsMenu.SetActive(false);
        QuitToTitleConformation.SetActive(false);
        ExitToDesktopConformation.SetActive(false);

        HUD_manager.GetComponent<HUDmanager>().hudEnabled = true;
        HUD_manager.GetComponent<HUDmanager>().showFullUI();
    }

    public void toPauseDefault()
    {
        PauseMenuDefault.SetActive(true);
        SettingsMenu.SetActive(false);
        QuitToTitleConformation.SetActive(false);
        ExitToDesktopConformation.SetActive(false);
    }
    
    public void openSettings()
    {
        PauseMenuDefault.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void quitToMenu()
    {
        PauseMenuDefault.SetActive(false);
        QuitToTitleConformation.SetActive(true);
    }

    public void exitToDesktop()
    {
        PauseMenuDefault.SetActive(false);
        ExitToDesktopConformation.SetActive(true);
    }

    public void confirmExitGame()
    {
        Debug.Log("Player exited the game");
    }

    public void confirmQuitToMenu()
    {
        Debug.Log("Player quit to menu");
    }
}
