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
        sync_tick = 1,
        auth_fail,
        auth_malformed,
        auth_success,
        create_player,
        remove_player,
        playermove,
        player_died,
        player_respawn,
    }

    // Client to sever id's
    public enum CTS : ushort
    {
        auth_attempt = 1,
        openworldloaded,
        inputs,
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

    private uint _serverTick;
    public uint ServerTick
    {
        get => _serverTick;
        private set
        {
            _serverTick = value;
            InterpolationTick = (uint)(value - TicksBetweenPositionUpdates);
        }
    }
    public uint InterpolationTick { get; private set; }
    private uint _ticksBetweenPositionUpdates = 2;
    public uint TicksBetweenPositionUpdates
    {
        get => _ticksBetweenPositionUpdates;
        private set
        {
            _ticksBetweenPositionUpdates = value;
            InterpolationTick = (uint)(ServerTick - value);
        }
    }

    [SerializeField] private uint TickDivergenceTolerance = 1;

    private void Start()
    {
        // RiptideLogging stuff
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        // Making game server client
        Client = new Client();

        ServerTick = 2;
    }

    private void FixedUpdate()
    {
        Client.Tick();
        ServerTick++;
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

    private void SetTick(uint serverTick)
    {
        if (Mathf.Abs(ServerTick - serverTick) > TickDivergenceTolerance)
        {
            Debug.Log($"tick {ServerTick} => {serverTick}");
            ServerTick = serverTick;
        }
    }

    [MessageHandler((ushort)Messages.STC.sync_tick)]
    private static void SyncTick(Message message)
    {
        Singleton.SetTick(message.GetUInt());
    }
}