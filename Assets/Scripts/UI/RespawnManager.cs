using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RiptideNetworking;

public class RespawnManager : MonoBehaviour
{
    private static RespawnManager _singleton;
    public static RespawnManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(RespawnManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private GameObject RespawnScreenHolder;
    [SerializeField] private TextMeshProUGUI respawnCountdownText;
    [SerializeField] private GameObject RespawnButton;
    private bool respawnScreenOpen = false;
    private bool doCountdown = false;
    public float respawnCountdown = 5;

    private void Awake()
    {
        Singleton = this;

        respawnScreenOpen = false;
        RespawnScreenHolder.SetActive(false);
    }

    private void Update()
    {
        if(respawnScreenOpen == true)
        {
            if (doCountdown == true)
            {
                respawnCountdown -= Time.deltaTime;
                int roundedValue = Mathf.RoundToInt(respawnCountdown);

                respawnCountdownText.text = "Respawn in " + roundedValue.ToString();
            }
        }

        if(respawnCountdown < 0)
        {
            RespawnButton.SetActive(true);
            doCountdown = false;

            respawnCountdownText.text = "";
        }
    }

    public void showRespawnScreen()
    {
        if (respawnScreenOpen == false)
        {
            respawnCountdown = 5;
            RespawnScreenHolder.SetActive(true);
            respawnScreenOpen = true;
            doCountdown = true;
        }
    }

    public void closeRespawnScreen()
    {
        if(respawnScreenOpen == true)
        {
            RespawnScreenHolder.SetActive(false);
            respawnScreenOpen = false;
        }
    }

    private void playerClickRespawn()
    {
        Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.playerClick_Respawn);
        NetworkManager.Singleton.Client.Send(message);
    }
}
