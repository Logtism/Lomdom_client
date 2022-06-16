using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

//Code for pulling values for Ammo, Health, and Money are commented out.
//Call the update function for the relevant interaction when making a value change e.g. updateMoney() when money value changes

public class HUDmanager : MonoBehaviour
{
    private static HUDmanager _singleton;
    public static HUDmanager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(HUDmanager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [Header("HUD settings")]
    public float fadeTime; //Time UI elements are visible for. Can be altered depending on context
    public int IncreaseRate; //Money increase animimation playback speed
    public bool hudEnabled = true; //Whether or not the HUD is eneabled. Can be turned off at any time and will disable the HUD

    [Header("HUD Elements")]
    [SerializeField] private GameObject MoneyHolder;
    [SerializeField] private GameObject AmmoHolder;
    [SerializeField] private GameObject MapHolder;
    [SerializeField] private GameObject HealthHolder;

    [Header("HUD Text elements")]
    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private TextMeshProUGUI AmmoText;

    [Header("HUD Image elements")]
    [SerializeField] private GameObject HealthBar;

    [Header("HUD Animators")]
    [SerializeField] private Animator MoneyAnimator;
    [SerializeField] private Animator HealthAnimator;
    [SerializeField] private Animator MapAnimator;
    [SerializeField] private Animator AmmoAnimator;

    [Header("HUD modifiers")]
    public bool lockVisibility = false; //Enable before calling the UI update you want to lock on screen, and then disable immeadietly after. Unlock by calling unlockVisibility() NOTE: will hide the entire HUD.

    //[Header("Player info manager")]
    //public GameObject playerInfoManager;

    private float currentMoney;
    private float currentMAXmoney;
    private float moneyUItimer;
    private bool moneyUIactive = true;

    private uint currentAmmoRound;
    private uint currentAmmoTotal;
    private float ammoUItimer;
    private bool ammoUIactive = true;

    private Image HealthBarImage;
    private const float maxHealth = 100f;
    private float health = maxHealth;
    private float healthUItimer;
    private bool healthUIactive = true;

    private float mapUItimer;
    private bool mapUIactive = true;

    private void Awake()
    {
        Singleton = this;
        //currentMAXmoney = playerInfoManager.GetComponent<PlayerInfo>().PlayerMoney;
        HealthBarImage = HealthBar.GetComponent<Image>();

        unlockVisibility();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            showFullUI();
        }

        MoneyText.text = "$" + currentMoney.ToString();

        if (currentMoney < currentMAXmoney)
        {
            int ScoreIncrement = IncreaseRate;
            currentMoney += ScoreIncrement;

            if (currentMoney > currentMAXmoney)
                currentMoney = currentMAXmoney;
        }

        if (currentMoney > currentMAXmoney)
        {
            int ScoreIncrement = IncreaseRate;
            currentMoney -= ScoreIncrement;

            if (currentMoney < currentMAXmoney)
                currentMoney = currentMAXmoney;
        }
    }

    public void updateMoney()
    {
        // currentMAXmoney = playerInfoManager.GetComponent<PlayerInfo>().PlayerMoney;
        currentMAXmoney = LevelManager.Singleton.LocalPlayer.Balance;

        if (!moneyUIactive && hudEnabled)
            StartCoroutine(moneyInteract());

        if (moneyUIactive == true && hudEnabled)
        {
            moneyUItimer = fadeTime;
            Debug.Log("updated money UI timer");
        }
    }

    public void updateAmmo()
    {
        if (WeaponWheel.Singleton.ActiveWeapon)
        {
            currentAmmoRound = WeaponWheel.Singleton.CurrectAmmo;
            currentAmmoTotal = WeaponWheel.Singleton.ActiveWeapon.MagCapacity;
        }
        else
        {
            currentAmmoRound = 0;
            currentAmmoTotal = 0;
        }


        AmmoText.text = currentAmmoRound.ToString() + "/" + currentAmmoTotal.ToString();

        if (!ammoUIactive && hudEnabled)
            StartCoroutine(ammoInteract());

        if (ammoUIactive == true && hudEnabled)
        {
            ammoUItimer = fadeTime;
            Debug.Log("updated ammo UI timer");
        }
    }

    public void updateHealth()
    {
        health = LevelManager.Singleton.LocalPlayer.Health;

        HealthBarImage.fillAmount = health / maxHealth;

        if (!healthUIactive && hudEnabled)
            StartCoroutine(healthInteract());

        if (healthUIactive == true && hudEnabled)
        {
            healthUItimer = fadeTime;
            Debug.Log("updated health UI timer");
        }
    }

    public void updateMap()
    {
        if (!mapUIactive && hudEnabled)
            StartCoroutine(mapInteract());

        if(mapUIactive == true && hudEnabled)
        { 
            mapUItimer = fadeTime;
            Debug.Log("updated map UI timer"); 
        }
    }

    public void playerDie()
    {
        hudEnabled = false;
        unlockVisibility();
    }

    public void unlockVisibility()
    {
        lockVisibility = false;


        if (moneyUIactive)
        {
            MoneyHolder.SetActive(false);
            moneyUIactive = false;
        }

        if (ammoUIactive)
        {
            AmmoHolder.SetActive(false);
            ammoUIactive = false;
        }

        if (mapUIactive)
        {
            MapHolder.SetActive(false);
            mapUIactive = false;
        }

        if (healthUIactive)
        {
            HealthHolder.SetActive(false);
            healthUIactive = false;
        }
    }

    public void showFullUI()
    {
        updateAmmo();
        updateHealth();
        updateMoney();
        updateMap();
    }

    public void forceHideHUD()
    {
        MoneyHolder.SetActive(false);
        AmmoHolder.SetActive(false);
        HealthHolder.SetActive(false);
        MapHolder.SetActive(false);
    }

    IEnumerator moneyInteract()
    {
        MoneyHolder.SetActive(true);
        moneyUIactive = true;
        MoneyAnimator.Play("MoneyIN", 0, 0.0f);

        if (!lockVisibility)
        {
            for (moneyUItimer = fadeTime; moneyUItimer >= 0; moneyUItimer -= 1)
            {
                Debug.Log(moneyUItimer);
                
                if (!hudEnabled)
                {
                    MoneyAnimator.Play("MoneyOUT", 0, 0.0f);

                    float animationLength = MoneyAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    MoneyHolder.SetActive(false);
                    moneyUIactive = false;
                    yield break;
                }

                if (moneyUItimer == 0)
                {
                    MoneyAnimator.Play("MoneyOUT", 0, 0.0f);

                    float animationLength = MoneyAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    MoneyHolder.SetActive(false);
                    moneyUIactive = false;
                }
                
                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator ammoInteract()
    {
        AmmoHolder.SetActive(true);
        ammoUIactive = true;
        AmmoAnimator.Play("AmmoIN", 0, 0.0f);

        if (!lockVisibility)
        {
            Debug.Log("log");

            for (ammoUItimer = fadeTime; ammoUItimer >= 0; ammoUItimer -= 1)
            {
                Debug.Log(ammoUItimer);
                
                if (!hudEnabled)
                {
                    AmmoAnimator.Play("AmmoOUT", 0, 0.0f);

                    float animationLength = AmmoAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    AmmoHolder.SetActive(false);
                    ammoUIactive = false;
                    yield break;
                }

                if (ammoUItimer == 0)
                {
                    AmmoAnimator.Play("AmmoOUT", 0, 0.0f);

                    float animationLength = AmmoAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    AmmoHolder.SetActive(false);
                    ammoUIactive = false;
                }
                
                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator mapInteract()
    {
        MapHolder.SetActive(true);
        mapUIactive = true;
        MapAnimator.Play("MapIN", 0, 0.0f);

        if (!lockVisibility)
        {
            for (mapUItimer = fadeTime; mapUItimer >= 0; mapUItimer -= 1)
            {
                Debug.Log(mapUItimer);

                if (!hudEnabled)
                {
                    MapAnimator.Play("MapOUT", 0, 0.0f);

                    float animationLength = MapAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    MapHolder.SetActive(false);
                    mapUIactive = false;
                    yield break;
                }

                if (mapUItimer == 0)
                {
                    MapAnimator.Play("MapOUT", 0, 0.0f);

                    float animationLength = MapAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    MapHolder.SetActive(false);
                    mapUIactive = false;
                }

                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator healthInteract()
    {
        HealthHolder.SetActive(true);
        healthUIactive = true;
        HealthAnimator.Play("HealthIN", 0, 0.0f);

        if (!lockVisibility)
        {
            for (healthUItimer = fadeTime; healthUItimer >= 0; healthUItimer -= 1)
            {
                Debug.Log(healthUItimer);
                
                if (!hudEnabled)
                {
                    HealthAnimator.Play("HealthOUT", 0, 0.0f);

                    float animationLength = HealthAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    HealthHolder.SetActive(false);
                    healthUIactive = false;
                    yield break;
                }

                if (healthUItimer == 0)
                {
                    HealthAnimator.Play("HealthOUT", 0, 0.0f);

                    float animationLength = HealthAnimator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSecondsRealtime(animationLength);

                    HealthHolder.SetActive(false);
                    healthUIactive = false;
                }
                
                yield return new WaitForSeconds(1);


            }
        }
    }
}