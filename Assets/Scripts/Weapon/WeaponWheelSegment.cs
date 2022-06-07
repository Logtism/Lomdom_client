using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheelSegment : MonoBehaviour
{
    [SerializeField] public Weapon weapon;
    [SerializeField] private GameObject Model;

    public void Select()
    {
        WeaponWheel.Singleton.ToggleWheel();
        WeaponWheel.Singleton.SwitchWeapon(weapon, Model);
    }
}
