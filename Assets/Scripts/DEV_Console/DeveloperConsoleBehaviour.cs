using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeveloperConsoleBehaviour : MonoBehaviour
{
    [SerializeField] private string prefix = string.Empty;
    [SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];

    [Header("UI")]
    [SerializeField] private GameObject uiCanvas = null;
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private TextMeshProUGUI logText;

    private static DeveloperConsoleBehaviour instance;

    private DeveloperConsole developerConsole;

    private DeveloperConsole DeveloperConsole
    {
        get
        {
            if(developerConsole != null) { return developerConsole; }
            return developerConsole = new DeveloperConsole(prefix, commands);
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleDevConsole()
    {
        if (uiCanvas.activeSelf)
        {
            uiCanvas.SetActive(false);
            PlayerMove.Singleton.canMove = true;
        }
        else
        {
            uiCanvas.SetActive(true);
            inputField.ActivateInputField();
            PlayerMove.Singleton.canMove = false;
        }
    }

    public void ProcessCommand(string inputValue)
    {
        DeveloperConsole.ProcessCommand(inputValue);

        inputField.text = string.Empty;
        inputField.ActivateInputField();

        logText.text = logText.text + "<br>" + inputValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            ToggleDevConsole();
        }

        if(Input.GetKeyDown(KeyCode.Return) && inputField.text != string.Empty)
        {
            ProcessCommand(inputField.text);
        }
    }

}
