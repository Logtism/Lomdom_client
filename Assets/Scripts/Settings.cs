using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Settings : MonoBehaviour
{
    private static Settings _singleton;
    public static Settings Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(Settings)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [Header("Settings ID")]
    [SerializeField] public string settingsId = "0000";
    
    [Header("For | General")]
    [SerializeField] public int enableHeadbob;
    [SerializeField] public int enableScreenshake;
    [SerializeField] public int enableAutoRespawn;
    [SerializeField] public int vehicleHud;
    [SerializeField] public int dynamicHud;
    [SerializeField] public int crosshairEnabled;
    [SerializeField] public int waypointsEnabled;
    [SerializeField] public int reduceUiMotion;
    [SerializeField] public int localUiLanguage;

    [Header("For | Audio")]
    [SerializeField] public int masterVolume;
    [SerializeField] public int musicVolume;
    [SerializeField] public int soundeffectsVolume;
    [SerializeField] public int enableAmbience;
    [SerializeField] public int enableVehicleMusic;
    [SerializeField] public int enableHeistMusic;

    [Header("For | Video")]
    [SerializeField] public int resolutionPreset;
    [SerializeField] public int graphicsPreset;
    [SerializeField] public int antiAliasing;
    [SerializeField] public int anisotropicFilteringEnabled;
    [SerializeField] public int enableRealtimeShadows;
    [SerializeField] public int shadowResolution;
    [SerializeField] public int enableSoftParticles;
    [SerializeField] public int enableRealtimeReflections;
    [SerializeField] public int targetFramerate;

    [Header("For | Online")]
    [SerializeField] public int enableServerChat;
    [SerializeField] public int saveLoginInfo;
    [SerializeField] public int enableNameTags;
    [SerializeField] public int serverRegion;
    [SerializeField] public int accountType;

    [Header("Runtime modifiers")]
    [SerializeField] public bool unsavedChanges;
    [SerializeField] public bool requireRestart;
    [SerializeField] public bool unappliedChanges;

    private void Awake()
    {
        Singleton = this;
        loadPlayerPrefs();
    }

    //private void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    savePlayerPrefs();
        //    Debug.Log("updated player save (loca)");
        //}
    //}

    public void loadPlayerPrefs()
    {
        //For general
        enableHeadbob = PlayerPrefs.GetInt("enableHeadbob", enableHeadbob);
        enableScreenshake = PlayerPrefs.GetInt("enableSS", enableScreenshake);
        enableAutoRespawn = PlayerPrefs.GetInt("enableAutoRespawn", enableAutoRespawn);
        vehicleHud = PlayerPrefs.GetInt("vehicleHUD", vehicleHud);
        dynamicHud = PlayerPrefs.GetInt("dynamicHUD", dynamicHud);
        crosshairEnabled = PlayerPrefs.GetInt("crosshairEnabled", crosshairEnabled);
        waypointsEnabled = PlayerPrefs.GetInt("waypointEnabled", waypointsEnabled);
        reduceUiMotion = PlayerPrefs.GetInt("reduceUiMotion", reduceUiMotion);
        localUiLanguage = PlayerPrefs.GetInt("localUiLanguage", localUiLanguage);

        //For audio
        masterVolume = PlayerPrefs.GetInt("masterVolume", masterVolume);
        musicVolume = PlayerPrefs.GetInt("musicVolume", musicVolume);
        soundeffectsVolume = PlayerPrefs.GetInt("soundeffectsVolume", soundeffectsVolume);
        enableAmbience = PlayerPrefs.GetInt("enableAmbience", enableAmbience);
        enableVehicleMusic = PlayerPrefs.GetInt("enableVehicleMusic", enableVehicleMusic);
        enableHeistMusic = PlayerPrefs.GetInt("enableHeistMusic", enableHeistMusic);

        //For video
        resolutionPreset = PlayerPrefs.GetInt("resolutionPreset", resolutionPreset);
        graphicsPreset = PlayerPrefs.GetInt("graphicsPreset", graphicsPreset);
        antiAliasing = PlayerPrefs.GetInt("antiAliasing", antiAliasing);
        anisotropicFilteringEnabled = PlayerPrefs.GetInt("anisotropicFilteringEnabled", anisotropicFilteringEnabled);
        enableRealtimeShadows = PlayerPrefs.GetInt("enableRealtimeShadows", enableRealtimeShadows);
        shadowResolution = PlayerPrefs.GetInt("shadowResolution", shadowResolution);
        enableSoftParticles = PlayerPrefs.GetInt("enableSoftParticles", enableSoftParticles);
        enableRealtimeReflections = PlayerPrefs.GetInt("enableRealtimeReflections", enableRealtimeReflections);
        targetFramerate = PlayerPrefs.GetInt("targetFramerate", targetFramerate);

        //For online
        enableServerChat = PlayerPrefs.GetInt("enableServerChat", enableServerChat);
        saveLoginInfo = PlayerPrefs.GetInt("saveLoginInfo", saveLoginInfo);
        enableNameTags = PlayerPrefs.GetInt("enableNameTags", enableNameTags);
        serverRegion = PlayerPrefs.GetInt("serverRegion", serverRegion);
        accountType = PlayerPrefs.GetInt("accountType", accountType);
    }

    public void savePlayerPrefs()
    {
        //For general
        PlayerPrefs.SetInt("enableHeadbob", enableHeadbob);
        PlayerPrefs.SetInt("enableSS", enableScreenshake);
        PlayerPrefs.SetInt("enableAutoRespawn", enableAutoRespawn);
        PlayerPrefs.SetInt("vehicleHUD", vehicleHud);
        PlayerPrefs.SetInt("dynamicHUD", dynamicHud);
        PlayerPrefs.SetInt("crosshairEnabled", crosshairEnabled);
        PlayerPrefs.SetInt("waypointEnabled", waypointsEnabled);
        PlayerPrefs.SetInt("reduceUiMotion", reduceUiMotion);
        PlayerPrefs.SetInt("localUiLanguage", localUiLanguage);

        //For audio
        PlayerPrefs.SetInt("masterVolume", masterVolume);
        PlayerPrefs.SetInt("musicVolume", musicVolume);
        PlayerPrefs.SetInt("soundeffectsVolume", soundeffectsVolume);
        PlayerPrefs.SetInt("enableAmbience", enableAmbience);
        PlayerPrefs.SetInt("enableVehicleMusic", enableVehicleMusic);
        PlayerPrefs.SetInt("enableHeistMusic", enableHeistMusic);

        //For video
        PlayerPrefs.SetInt("resolutionPreset", resolutionPreset);
        PlayerPrefs.SetInt("graphicsPreset", graphicsPreset);
        PlayerPrefs.SetInt("antiAliasing", antiAliasing);
        PlayerPrefs.SetInt("anisotropicFilteringEnabled", anisotropicFilteringEnabled);
        PlayerPrefs.SetInt("enableRealtimeShadows", enableRealtimeShadows);
        PlayerPrefs.SetInt("shadowResolution", shadowResolution);
        PlayerPrefs.SetInt("enableSoftParticles", enableSoftParticles);
        PlayerPrefs.SetInt("enableRealtimeReflections", enableRealtimeReflections);
        PlayerPrefs.SetInt("targetFramerate", targetFramerate);

        //For online
        PlayerPrefs.SetInt("enableServerChat", enableServerChat);
        PlayerPrefs.SetInt("saveLoginInfo", saveLoginInfo);
        PlayerPrefs.SetInt("enableNameTags", enableNameTags);
        PlayerPrefs.SetInt("serverRegion", serverRegion);
        PlayerPrefs.SetInt("accountType", accountType);

        //For all --
        PlayerPrefs.Save();
    }

    public void applyPlayerPrefs()
    {

    }
}
