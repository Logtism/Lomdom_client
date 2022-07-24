using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using RiptideNetworking;

public class ChatUI : MonoBehaviour
{
    private static ChatUI _singleton;
    public static ChatUI Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(ChatUI)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] private GameObject Menu;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private TMP_Text textbox;
    [SerializeField] private TMP_InputField inputField;

    public bool ChatOpen = false;

    private float chatTimer = 0;

    private void OpenChat()
    {
        Menu.SetActive(true);
        CameraLook.Singleton.ToggleCursorMode();
        PlayerMove.Singleton.canMove = false;
        PlayerCameraDynamics.Singleton.headbobEnabled = false;
        scroll.verticalNormalizedPosition = 0f;
        ChatOpen = true;
        inputField.ActivateInputField();
    }

    private void CloseChat()
    {
        CameraLook.Singleton.ToggleCursorMode();
        PlayerMove.Singleton.canMove = true;
        PlayerCameraDynamics.Singleton.headbobEnabled = true;
        Menu.SetActive(false);
        StartCoroutine(closeChatCooldown());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !ChatOpen && PauseManager.Singleton.menuIsOpen == false && MissionMenuManager.Singleton.missionMenuOpen == false)
        {
            OpenChat();
        }

        if (ChatOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChat();
        }

        if (ChatOpen && Input.GetKeyDown(KeyCode.Return))
        {
            sendMsg();
        }
    }

    public void AddMessage(string msg)
    {
        textbox.text = textbox.text + "<br>" + msg;
        scroll.verticalNormalizedPosition = 0f;
    }

    [MessageHandler((ushort)Messages.STC.send_chat_msg)]
    private static void SendChatMsg(Message message)
    {
        string msg = message.GetString();
        Singleton.AddMessage(msg);
    }

    public void sendMsg()
    {
        if (inputField.text.Length <= 500 && inputField.text.Length > 0 && chatTimer == 0)
        {
            Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.send_chat_msg);
            message.AddString(inputField.text);
            NetworkManager.Singleton.Client.Send(message);
            inputField.text = "";
            StartCoroutine(chatCooldown());
        }
        if(inputField.text.Length > 500)
        {
            AddMessage("Msg can not be over 500 chars.");
        }
        inputField.ActivateInputField();
    }

    private IEnumerator chatCooldown()
    {
        chatTimer = 1;
        yield return new WaitForSeconds(1);
        chatTimer = 0;
    }

    private IEnumerator closeChatCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        ChatOpen = false;
    }
}
