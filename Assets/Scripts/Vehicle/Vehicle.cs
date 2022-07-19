using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public uint id;
    private Vehicle_data Vehicle_Data;
    [SerializeField] public GameObject cam;
    [SerializeField] public Interpolator interpolator;
    public void SetType(Vehicle_data vehicle_Data, uint id)
    {
        Vehicle_Data = vehicle_Data;
        this.id = id;
    }
}
