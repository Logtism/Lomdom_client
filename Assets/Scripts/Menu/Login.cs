using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    private static Login _singleton;
    public static Login Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(Login)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public TMP_InputField username_input;
    public TMP_InputField password_input;
    public Button login_btn;
    public TMP_Text error_text;

    public void Auth()
    {
        // Making the ui uninteractable so user cam't change info after clicking login
        username_input.interactable = false;
        password_input.interactable = false;
        login_btn.interactable = false;

        // Checking that the username and password fields are not empty
        if (username_input.text != "" && password_input.text != "")
        {
            string error_msg = AuthManager.Singleton.GetAuthToken(username_input.text, password_input.text);
            if (error_msg == "")
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                ResetUI(error_msg);
            }
        }
        else
        {
            ResetUI("Username and password are requirded");
        }
    }

    public void ResetUI(string error_msg = null)
    {
        username_input.interactable = true;
        password_input.interactable = true;
        login_btn.interactable = true;
        if (error_msg != null)
        {
            error_text.text = error_msg;
        }
    }

    private void Start()
    {
        error_text.text = AuthManager.Singleton.error_msg;
        AuthManager.Singleton.error_msg = "";
    }
}
