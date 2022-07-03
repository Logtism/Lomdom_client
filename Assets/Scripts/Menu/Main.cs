using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class Confirm : UnityEvent { }

[System.Serializable]
public class Deny : UnityEvent { }

public class Main : MonoBehaviour
{
    [Header("Menu elements")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private PopUp popUpOnExitGame;
    [SerializeField] private Button[] Menu_MainButtons;
    [SerializeField] private GameObject[] MenuPanels;
    private bool isOnMainMenu = true;

    [Header("PopUp - Dialog box")]
    [SerializeField] private GameObject DialogBoxMain;
    [SerializeField] private GameObject confirmButtons;
    [SerializeField] private GameObject awknoledgeButtons;

    [SerializeField] private TextMeshProUGUI PopUpTitle;
    [SerializeField] private TextMeshProUGUI PopUpMsg;

    [Header("PopUp - conformation box only")]
    [SerializeField] private TextMeshProUGUI popUpBttn1;
    [SerializeField] private TextMeshProUGUI popUpBttn2;

    [Header("PopUp - awknoledge box only")]
    [SerializeField] private TextMeshProUGUI popUpBttn3;

    private bool dialogPopUpOpen = false;
    private PopUp currentDisplayPopUp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOnMainMenu == false) { closeAllMenus(); }

            else { dialogPopUp(popUpOnExitGame); }
        }
    }

    public void Play()
    {
        NetworkManager.Singleton.Connect();
    }

    public void switchActiveMenu(GameObject menuPanel)
    {
        menuPanel.SetActive(true);

        for (int i = 0; i < MenuPanels.Length; i++)
        {
            if(MenuPanels[i] != menuPanel)
            {
                MenuPanels[i].SetActive(false);
            }
        }

        if(isOnMainMenu == true)
        {
            isOnMainMenu = false;
            //MainMenuPanel.SetActive(false);
        }
    }

    public void closeAllMenus()
    {
        isOnMainMenu = true;
        //MainMenuPanel.SetActive(true);

        for (int i = 0; i < MenuPanels.Length; i++)
        {
            MenuPanels[i].SetActive(false);
        }
    }

    //--------------------------- Dialog popUp system below to end

    public void dialogPopUp(PopUp popUp)
    {
        if (isOnMainMenu)
        {
            for (int i = 0; i < Menu_MainButtons.Length; i++)
            {
                Menu_MainButtons[i].interactable = false;
            }
        }
        
        currentDisplayPopUp = popUp;
        
        PopUpTitle.text = popUp.DialogTitle;
        PopUpMsg.text = popUp.DialogMsg;
        PopUpMsg.color = popUp.DialogMsgColor;
        
        if (currentDisplayPopUp.dialogType == dialogType.Confirm)
        {
            confirmButtons.SetActive(true);
            awknoledgeButtons.SetActive(false);

            popUpBttn1.text = popUp.ConfirmButtonText;
            popUpBttn2.text = popUp.DenyButtonText;
        }

        else
        {
            confirmButtons.SetActive(false);
            awknoledgeButtons.SetActive(true);
            
            popUpBttn3.text = popUp.ConfirmButtonText;
        }        
        
        DialogBoxMain.SetActive(true);
        dialogPopUpOpen = true;

    }

    public void closeDialogPopUp()
    {
        currentDisplayPopUp = null;
        DialogBoxMain.SetActive(false);
        dialogPopUpOpen = false;

        if (isOnMainMenu)
        {
            for (int i = 0; i < Menu_MainButtons.Length; i++)
            {
                Menu_MainButtons[i].interactable = true;
            }
        }
    }

    public void popUpConfirm()
    {
        if (dialogPopUpOpen)
        {
            currentDisplayPopUp.OnConfirmFunction.Invoke();
            closeDialogPopUp();
        }
    }

    public void popUpDeny()
    {
        if (dialogPopUpOpen)
        {
            if (currentDisplayPopUp.OnDialogDenyFunction != null)
            {
                currentDisplayPopUp.OnDialogDenyFunction.Invoke();
            }
            
            closeDialogPopUp();
        }
    }

    //---------------------------
}
