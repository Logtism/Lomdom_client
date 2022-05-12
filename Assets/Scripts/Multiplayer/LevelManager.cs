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
    private Dictionary<string, Player> Players = new Dictionary<string, Player>();

    public void Start()
    {
        Loading.Singleton.EnableLoading();
        StartCoroutine("EnableOnLoad");
    }

    [MessageHandler((ushort)Messages.STC.create_player)]
    private static void CreatePlayer(Message message)
    {
        string username = message.GetString();
        if (AuthManager.Singleton.username == username)
        {
            GameObject player_gameobject = Instantiate(Singleton.LocalPlayerPrefab, message.GetVector3(), message.GetQuaternion());
            Singleton.Players.Add(username, player_gameobject.GetComponent<Player>());

            Loading.Singleton.DisableLoading();
        }
        else
        {
            GameObject player_gameobject = Instantiate(Singleton.ForeignPlayerPrefab, message.GetVector3(), message.GetQuaternion());
            Singleton.Players.Add(username, player_gameobject.GetComponent<Player>());
        }

        GameObject player = Singleton.Players[username].gameObject;

        player.name = $"Player - {username}";
    }

    [MessageHandler((ushort)Messages.STC.playermove)]
    private static void PlayerMove(Message message)
    {
        if (Singleton.Players.TryGetValue(message.GetString(), out Player player))
        {
            player.Move(message.GetUInt(), message.GetVector3(), message.GetVector3());
        }
    }
}
