using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeistInfoManager : MonoBehaviour
{
    private static HeistInfoManager _singleton;
    public static HeistInfoManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(HeistInfoManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private TextMeshProUGUI HeistTypeText;
    [SerializeField] private TextMeshProUGUI HeistNameText;
   
    //public Heist heist;

    public bool heistInfoVisible;
   
    [SerializeField] private Animator HeistType;
    [SerializeField] private Animator HeistName;
    [SerializeField] private Animator SetupHeist;

    [SerializeField] private GameObject animationMask;

    private void Awake()
    {
        animationMask.SetActive(false);

        HeistType.gameObject.SetActive(false);
        HeistName.gameObject.SetActive(false);
        SetupHeist.gameObject.SetActive(false);

        heistInfoVisible = false;

        Singleton = this;
    }

    public void showHeistInfo()
    {
        //HeistNameText.text = heist.HeistName;
        //HeistTypeText.text = heist.HeistType;

        animationMask.SetActive(true);

        HeistType.gameObject.SetActive(true);
        HeistName.gameObject.SetActive(true);
        SetupHeist.gameObject.SetActive(true);

        heistInfoVisible = true;

        HeistType.Play("HeistTypeIN", 0, 0.0f);
        HeistName.Play("HeistNameIN", 0, 0.0f);
        SetupHeist.Play("SetupHeistIN", 0, 0.0f);
    }

    public void hideHeistInfo()
    {

        HeistType.Play("HeistTypeOUT", 0, 0.0f);
        HeistName.Play("HeistNameOUT", 0, 0.0f);
        SetupHeist.Play("SetupHeistOUT", 0, 0.0f);

        heistInfoVisible = false;

        //animationMask.SetActive(false);
    }

    public void forceHideHeistInfo()
    {
        HeistType.gameObject.SetActive(false);
        HeistName.gameObject.SetActive(false);
        SetupHeist.gameObject.SetActive(false);
    }
}
