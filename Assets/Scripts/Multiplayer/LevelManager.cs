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
    private Dictionary<string, GameObject> Players = new Dictionary<string, GameObject>();

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
            Singleton.Players.Add(username, Instantiate(Singleton.LocalPlayerPrefab, message.GetVector3(), message.GetQuaternion()));

            Loading.Singleton.DisableLoading();
        }
        else
        {
            Singleton.Players.Add(username, Instantiate(Singleton.ForeignPlayerPrefab, message.GetVector3(), message.GetQuaternion()));
        }

        GameObject player = Singleton.Players[username];

        player.name = $"Player - {username}";
    }

    [MessageHandler((ushort)Messages.STC.testing_r)]
    private static void TestingR (Message message)
    {
        Debug.Log("reliable");
    }

    [MessageHandler((ushort)Messages.STC.testing_ur)]
    private static void TestingUR(Message message)
    {
        Debug.Log("unreliable");
    }

}
