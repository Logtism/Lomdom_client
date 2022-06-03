using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RiptideNetworking;
using RiptideNetworking.Utils;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _singleton;
    public static LevelManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(LevelManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    IEnumerator EnableOnLoad()
    {
        while (Singleton.LocalPlayerPrefab == null)
        {
            Debug.Log(Singleton.LocalPlayerPrefab);
            yield return null;
        }
     
        Message openworldlevelloaded = Message.Create(MessageSendMode.reliable, Messages.CTS.openworldloaded);
        NetworkManager.Singleton.Client.Send(openworldlevelloaded);
    }

    [Header("Prefabs")]
    [SerializeField] private GameObject LocalPlayerPrefab;
    [SerializeField] private GameObject ForeignPlayerPrefab;
    private Dictionary<ushort, Player> Players = new Dictionary<ushort, Player>();

    public void Start()
    {
        Loading.Singleton.EnableLoading();
        StartCoroutine("EnableOnLoad");
    }

    [MessageHandler((ushort)Messages.STC.create_player)]
    private static void CreatePlayer(Message message)
    {
        string username = message.GetString();
        ushort ClientID = message.GetUShort();
        if (AuthManager.Singleton.ClientID == ClientID)
        {
            GameObject player_gameobject = Instantiate(Singleton.LocalPlayerPrefab, message.GetVector3(), message.GetQuaternion());
            Singleton.Players.Add(ClientID, player_gameobject.GetComponent<Player>());

            Loading.Singleton.DisableLoading();
        }
        else
        {
            GameObject player_gameobject = Instantiate(Singleton.ForeignPlayerPrefab, message.GetVector3(), message.GetQuaternion());
            Singleton.Players.Add(ClientID, player_gameobject.GetComponent<Player>());
        }

        GameObject player = Singleton.Players[ClientID].gameObject;

        player.name = $"Player - {ClientID}:{username}";
    }

    [MessageHandler((ushort)Messages.STC.remove_player)]
    private static void RemovePlayer(Message message)
    {
        ushort ClientID = message.GetUShort();
        Destroy(Singleton.Players[ClientID].gameObject);
        Singleton.Players.Remove(ClientID);
    }

    [MessageHandler((ushort)Messages.STC.playermove)]
    private static void PlayerMove(Message message)
    {
        if (Singleton.Players.TryGetValue(message.GetUShort(), out Player player))
        {
            player.Move(message.GetUInt(), message.GetVector3(), message.GetVector3());
        }
    }
}