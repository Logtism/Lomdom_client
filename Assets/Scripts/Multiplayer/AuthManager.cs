using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using RiptideNetworking;
using RiptideNetworking.Utils;

public class AuthManager : MonoBehaviour
{
    private static AuthManager _singleton;
    public static AuthManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(AuthManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public string API_URL { get; private set; } = "http://127.0.0.1:8000/api/client";
    public string username { get; private set; }
    public ushort ClientID { get; private set; }
    private string auth_token;

    public string error_msg = "";


    public void ReLogin()
    {
        username = "";
        auth_token = "";
        error_msg = "Authentication failed when trying to find a server";
        SceneManager.LoadScene("LoginMenu");
    }

    public string GetAuthToken(string username_, string password)
    {
        // Sending the request to get a auth token
        (int status_code, JObject json_content) r = Request.Post($"{API_URL}/get_auth_token/", $"'username': '{username_}', 'password': '{password}'");
        if (r.status_code == 200)
        {
            username = username_;
            auth_token = r.json_content["auth_token"].ToString();
            if ((bool)r.json_content["already_active_tokens"] == true)
            {
                error_msg = "Your account already was logged in so any other computers where you are logged in have been logged out";
            }
            return "";
        }
        else if (r.status_code == 401)
        {
            return "Username and password do not match.";
        }
        else
        {
            return "Something went wrong please try again.";
        }
    }

    public void AuthWithServer()
    {
        Message auth_msg = Message.Create(MessageSendMode.reliable, Messages.CTS.auth_attempt);
        auth_msg.AddStrings(new string[] { username, auth_token });
        NetworkManager.Singleton.Client.Send(auth_msg);
    }

    public string FindServer()
    {
        (int status_code, JObject json_content) r = Request.Post($"{API_URL}/find_server/", $"'username': '{username}', 'auth_token': '{auth_token}'");
        if (r.status_code == 200)
        {
            return $"{r.json_content["ip"]}:{r.json_content["port"]}";
        }
        // If the auth_token is invalid or does not exist make the user relogin
        else if (r.json_content.ContainsKey("error_msg"))
        {
            if (r.json_content["error_msg"].ToString() == "Auth token does not belong to username.")
            {
                ReLogin();
            }
        }
        else if (r.json_content.ContainsKey("detail"))
        {
            if (r.json_content["detail"].ToString() == "Not found.")
            {
                ReLogin();
            }
        }
        return "";
    }

    [MessageHandler((ushort)Messages.STC.auth_success)]
    private static void AuthSuccess(Message message)
    {
        Debug.Log("login success");
        Singleton.ClientID = message.GetUShort();
        SceneManager.LoadScene("OpenWorld");
    }

    [MessageHandler((ushort)Messages.STC.auth_fail)]
    private static void AuthFail(Message message)
    {
        NetworkManager.Singleton.Client.Disconnect();
        SceneManager.LoadScene("LoginMenu");
        Singleton.error_msg = "Failed to authenticate.";
    }

    [MessageHandler((ushort)Messages.STC.auth_malformed)]
    private static void AuthMalformed(Message message)
    {
        NetworkManager.Singleton.Client.Disconnect();
        SceneManager.LoadScene("LoginMenu");
        Singleton.error_msg = "Something went wrong please try again.";
    }

    public void OnApplicationQuit()
    {
        if (auth_token != null)
        {
            Request.Post($"{API_URL}/remove_auth_token/", $"'username': '{username}', 'auth_token': '{auth_token}'");
        }
        NetworkManager.Singleton.Client.Disconnect();
    }
}
