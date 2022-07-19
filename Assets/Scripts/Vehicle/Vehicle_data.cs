using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vehicle", menuName = "Vehicle")]
public class Vehicle_data : ScriptableObject
{
    [SerializeField] public GameObject Prefab;
    // Occupants do not include driver
    [SerializeField] public ushort MaxOccupants;
    [SerializeField] public bool FourWheelDrive;
    [SerializeField] public float Acceleration;
    [SerializeField] public float BrakingForce;
}
