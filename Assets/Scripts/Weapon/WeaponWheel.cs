using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;

public class WeaponWheel : MonoBehaviour
{
    private static WeaponWheel _singleton;
    public static WeaponWheel Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(WeaponWheel)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] private Transform Xrot;

    [SerializeField] private GameObject[] WheelSegments;
    private bool WheelActive = false;

    private GameObject ActiveModel;
    public Weapon ActiveWeapon;

    private float LastShotTimer;
    public uint CurrectAmmo;
    private bool Reloading = false;
    private float ReloadTimer;
    public void ToggleWheel()
    {
        WheelActive = !WheelActive;

        foreach (GameObject wheelsement in WheelSegments)
        {
            wheelsement.SetActive(WheelActive);
        }

        if (WheelActive && Cursor.lockState == CursorLockMode.Locked)
        {
            CameraLook.Singleton.ToggleCursorMode();
        }
        if (!WheelActive && Cursor.lockState == CursorLockMode.None)
        {
            CameraLook.Singleton.ToggleCursorMode();
        }
    }

    public void SwitchWeapon(Weapon weapon, GameObject model)
    {
        if (ActiveModel)
        {
            ActiveModel.SetActive(false);
        }
        if (weapon)
        {
            ActiveWeapon = weapon;
            CurrectAmmo = ActiveWeapon.MagCapacity;
            if (model)
            {
                ActiveModel = model;
                model.SetActive(true);
            }
            Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.weapon_switch);
            message.AddInt(ActiveWeapon.ID);
            NetworkManager.Singleton.Client.Send(message);
            HUDmanager.Singleton.updateAmmo();
        }
        else
        {
            ActiveWeapon = null;
        }
    }


    private void Fire()
    {
        if (ActiveWeapon && !Reloading)
        {
            if (LastShotTimer >= ActiveWeapon.RateOfFire && CurrectAmmo > 0)
            {
                Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.attack);
                message.AddVector3(Xrot.forward);
                NetworkManager.Singleton.Client.Send(message);
                CurrectAmmo--;
                HUDmanager.Singleton.updateAmmo();
                LastShotTimer = 0f;
            }
            else if (CurrectAmmo == 0)
            {
                Reload();
            }
        }
    }

    private void Reload()
    {
        if (ActiveWeapon && !Reloading)
        {
            Reloading = true;
            Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.weapon_reload);
            NetworkManager.Singleton.Client.Send(message);
        }
    }

    private void Update()
    {
        LastShotTimer += Time.deltaTime;
        if (Reloading)
        {
            ReloadTimer += Time.deltaTime;
            if (ReloadTimer >= ActiveWeapon.ReloadTime)
            {

                ReloadTimer = 0f;
                CurrectAmmo = ActiveWeapon.MagCapacity;
                Reloading = false;
                HUDmanager.Singleton.updateAmmo();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && PauseManager.Singleton.menuIsOpen == false)
        {
            ToggleWheel();
        }

        if (Input.GetButton("Fire1") && PauseManager.Singleton.menuIsOpen == false)
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R) && PauseManager.Singleton.menuIsOpen == false)
        {
            Reload();
        }
    }
}
