using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New mission", menuName = "Mission")]
public class Mission : ScriptableObject
{
    public string MissionName;
    [SerializeField] public MissionStart MissionStartFunction = new MissionStart();
    [SerializeField] public MissionEnd MissionEndFunction = new MissionEnd();
}
