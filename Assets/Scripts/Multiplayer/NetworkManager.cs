using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;

public class Messages
{
    // Server to client id's
    public enum STC : ushort
    {
        auth_fail = 1,
        auth_malformed,
        auth_success,
        create_player,
        testing_r,
        testing_ur
    }

    // Client to sever id's
    public enum CTS : ushort
    {
        auth_attempt = 1,
        openworldloaded,
    }
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
        string user = AuthManager.Singleton.error_msg;
    }

    public Client Client { get; private set; }

    private void Start()
    {
        // RiptideLogging stuff
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        // Making game server client
        Client = new Client();
    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    public void Connect()
    {
        string server_addr = AuthManager.Singleton.FindServer();
        if (server_addr != "")
        {
            Client.Connect(server_addr);
            AuthManager.Singleton.AuthWithServer();
        }
    }
}
