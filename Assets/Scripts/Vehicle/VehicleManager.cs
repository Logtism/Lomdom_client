using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class VehicleManager : MonoBehaviour
{
    private static VehicleManager _singleton;
    public static VehicleManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(VehicleManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] private Vehicle_data[] VehiclesTypes;
    private Dictionary<uint, Vehicle> vehicles = new Dictionary<uint, Vehicle>();
    private bool LocalPlayerCanEnterVehicle;

    private void Update()
    {
        if (LevelManager.Singleton.LocalPlayer)
        {
            if (LocalPlayerCanEnterVehicle && !LevelManager.Singleton.LocalPlayer.InVehicle && Input.GetKeyDown(KeyCode.K))
            {
                Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.enter_vehicle);
                NetworkManager.Singleton.Client.Send(message);
            }
            if (LevelManager.Singleton.LocalPlayer.InVehicle && Input.GetKeyDown(KeyCode.K))
            {
                Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.leave_vehicle);
                NetworkManager.Singleton.Client.Send(message);
            }
        }
    }

    [MessageHandler((ushort)Messages.STC.spawn_vehicle)]
    private static void SpawnVehicle(Message message)
    {
        int vehicle_type = message.GetInt();
        uint vehicleid = message.GetUInt();
        Vector3 position = message.GetVector3();
        Quaternion rotation = message.GetQuaternion();

        GameObject vehicle_gameobject = Instantiate(Singleton.VehiclesTypes[vehicle_type].Prefab, position, rotation);
        Vehicle vehicle = vehicle_gameobject.GetComponent<Vehicle>();
        vehicle.SetType(Singleton.VehiclesTypes[vehicle_type], vehicleid);
        Singleton.vehicles.Add(vehicleid, vehicle);
    }

    [MessageHandler((ushort)Messages.STC.despawn_vehicle)]
    private static void DespawnVehicle(Message message)
    {
        uint vehicleid = message.GetUInt();
        Vehicle vehicle = Singleton.vehicles[vehicleid];
        Singleton.vehicles.Remove(vehicleid);
        Destroy(vehicle.gameObject);
    }

    [MessageHandler((ushort)Messages.STC.can_enter_vehicle)]
    private static void CanEnterVehicle(Message message)
    {
        Singleton.LocalPlayerCanEnterVehicle = true;
    }

    [MessageHandler((ushort)Messages.STC.cannot_enter_vehicle)]
    private static void CannotEnterVehicle(Message message)
    {
        Singleton.LocalPlayerCanEnterVehicle = false;
    }

    [MessageHandler((ushort)Messages.STC.entered_vehicle_driver)]
    private static void EnteredVehicleDriver(Message message)
    {
        ushort clientid = message.GetUShort();
        uint vehicleid = message.GetUInt();

        Player player = LevelManager.Singleton.GetPlayer(clientid);
        Debug.Log(Singleton.vehicles);
        Vehicle vehicle = Singleton.vehicles[vehicleid];

        player.InVehicle = true;
        player.vehicle = vehicle;

        Destroy(vehicle.gameObject.GetComponent<Rigidbody>());

        if (player.IsLocal)
        {
            vehicle.cam.SetActive(true);
            player.CamTransform.gameObject.SetActive(false);
            player.model.SetActive(false);
            player.gameObject.GetComponent<CameraLook>().enabled = false;
            // Change movement
        }
        
        else
        {
            player.gameObject.SetActive(false);

        }
    }

    [MessageHandler((ushort)Messages.STC.entered_vehicle_passenger)]
    private static void EnteredVehiclePassenger(Message message)
    {
        ushort clientid = message.GetUShort();
        uint vehicleid = message.GetUInt();
    }

    [MessageHandler((ushort)Messages.STC.left_vehicle)]
    private static void LeftVehicle(Message message)
    {
        ushort clientid = message.GetUShort();

        Player player = LevelManager.Singleton.GetPlayer(clientid);

        player.model.SetActive(true);
        player.gameObject.GetComponent<CameraLook>().enabled = true;
        player.CamTransform.gameObject.SetActive(true);

        Rigidbody rb = player.vehicle.gameObject.AddComponent<Rigidbody>();
        rb.mass = 1000;

        player.vehicle.cam.SetActive(false);

        player.InVehicle = false;
        player.vehicle = null;
    }
}
