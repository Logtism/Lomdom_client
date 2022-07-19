using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Transform CamTransform;
    [SerializeField] public GameObject model;
    [SerializeField] private Interpolator interpolator;

    public bool IsLocal;
    public int Health;
    public float Balance = 0f;
    public bool InVehicle;
    public Vehicle vehicle;
    public Quaternion vehiclerotation;
    public void Move(uint tick, Vector3 NewPosition, Vector3 forward)
    {
        interpolator.NewUpdate(tick, NewPosition);

        if (IsLocal == false)
        {
            // Used for rotation but its a bit fucked so yeah.
            // CamTransform.forward = forward;
        }
    }

    public void MoveVehicle(uint tick, Vector3 NewPosition, Quaternion rotation)
    {
        vehicle.interpolator.NewUpdate(tick, NewPosition);

        vehiclerotation = rotation;
    }

    public void Update()
    {
        if (vehicle && vehiclerotation != null)
        {
            vehicle.transform.rotation = Quaternion.Slerp(vehicle.transform.rotation, vehiclerotation, 4f * Time.deltaTime);
        }
    }
}
