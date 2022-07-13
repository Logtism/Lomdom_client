using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Extra : MonoBehaviour
{
    private int completedTutorial = 0;
    private bool completedTut = false;

    private void Awake()
    {
        completedTutorial = PlayerPrefs.GetInt("completedTutorial", completedTutorial);
        
        if(completedTutorial == 1) { completedTut = true; }
        else { completedTut = false; }
    }

    private void Start()
    {
        if (completedTut == true) { Main.Singleton.enableOnlinePlay(); }
    }
}
