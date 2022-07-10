using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Main_Functions : MonoBehaviour
{
    public void quitGame()
    {
        Application.Quit();
    }

    public void restartApp()
    {
        Process.Start(Application.dataPath + "/../YourGameName.exe");
        Application.Quit();
    }

    public void connectOnline()
    {
        NetworkManager.Singleton.Connect();
    }

    public void closePopUp()
    {
        Main.Singleton.closeDialogPopUp();
    }

    public void redirectToNews()
    {
        closePopUp();
        Application.OpenURL("https://5qgn3nwglr.pythonanywhere.com/home/");
    }
}
