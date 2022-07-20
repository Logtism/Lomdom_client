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

    private bool ChatOpen = false;

    private void OpenChat()
    {
        Menu.SetActive(true);
        CameraLook.Singleton.ToggleCursorMode();
        PlayerMove.Singleton.canMove = false;
        scroll.verticalNormalizedPosition = 0f;
        ChatOpen = true;
        inputField.ActivateInputField();
    }

    private void CloseChat()
    {
        CameraLook.Singleton.ToggleCursorMode();
        PlayerMove.Singleton.canMove = true;
        Menu.SetActive(false);
        ChatOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !ChatOpen)
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
        if (inputField.text.Length <= 500 && inputField.text.Length > 0)
        {
            Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.send_chat_msg);
            message.AddString(inputField.text);
            NetworkManager.Singleton.Client.Send(message);
            inputField.text = "";
        }
        else
        {
            AddMessage("Msg can not be over 500 chars.");
        }
        inputField.ActivateInputField();
    }
}
