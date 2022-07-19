using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnVehicleCommand", menuName = "Utilities/DeveloperCommands/SpawnVehicle")]
public class SpawnVehicleCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        Debug.Log("test");

        return true;
    }
}
