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
        if (weapon && model)
        {
            ActiveWeapon = weapon;
            CurrectAmmo = ActiveWeapon.MagCapacity;
            ActiveModel = model;
            model.SetActive(true);
            Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.weapon_switch);
            message.AddInt(ActiveWeapon.ID);
            NetworkManager.Singleton.Client.Send(message);
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
            if (LastShotTimer >= ActiveWeapon.RateOfFire)
            {
                Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.attack);
                message.AddVector3(Xrot.forward);
                NetworkManager.Singleton.Client.Send(message);
                LastShotTimer = 0f;
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
                CurrectAmmo = ActiveWeapon.MagCapacity;
                Reloading = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWheel();
        }

        if (Input.GetButton("Fire1"))
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
}
