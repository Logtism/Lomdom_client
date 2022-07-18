using UnityEngine;

[CreateAssetMenu(fileName = "NewLogCommand", menuName = "Utilities/DeveloperCommands/DebugLogCommand")]
public class DebugLogCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        string logText = string.Join(" ", args);

        if (logText == "nigger23")
            Debug.Log(logText);

        else
            Debug.Log("InvalidCommand");

        return true;
    }
}
