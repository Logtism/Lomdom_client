using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private static Loading _singleton;
    public static Loading Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(Loading)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public GameObject LoadingUI;

    public void EnableLoading()
    {
        LoadingUI.SetActive(true);
    }

    public void DisableLoading()
    {
        LoadingUI.SetActive(false);
    }
}
