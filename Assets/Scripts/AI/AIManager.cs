using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public enum AIMode
{
    waypoint = 1,
    patrol,
}

public enum AiId : int
{
    test_civ = 0,
}

public class AIManager : MonoBehaviour
{
    private static AIManager _singleton;
    public static AIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(AIManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] private GameObject[] AiPrefabs = new GameObject[] { };
    private Dictionary<int, AI> ais = new Dictionary<int, AI>();

    [MessageHandler((ushort)Messages.STC.spawn_waypoint_ai)]
    private static void SpawnWaypointAI(Message message)
    {
        GameObject prefab = Singleton.AiPrefabs[message.GetInt()];
        int ai_id = message.GetInt();

        GameObject instance = Instantiate(prefab, message.GetVector3(), Quaternion.identity);
        AI ai = instance.GetComponent<AI>();
        ai.SetInfoWaypoint(ai_id, message.GetVector3());
        Singleton.ais[ai_id] = ai;
    }

    [MessageHandler((ushort)Messages.STC.spawn_patrol_ai)]
    private static void SpawnPatrolAI(Message message)
    {
        GameObject prefab = Singleton.AiPrefabs[message.GetInt()];
        int ai_id = message.GetInt();

        GameObject instance = Instantiate(prefab, message.GetVector3(), Quaternion.identity);
        AI ai = instance.GetComponent<AI>();
        ai.SetInfoPatrol(ai_id, message.GetVector3());
        Singleton.ais[ai_id] = ai;
    }

    [MessageHandler((ushort)Messages.STC.kill_ai)]
    private static void KillAI(Message message)
    {
        int id = message.GetInt();
        AI ai = Singleton.ais[id];
        Destroy(ai.gameObject);
        Singleton.ais.Remove(id);
    }

    [MessageHandler((ushort)Messages.STC.sync_ai_pos)]
    private static void SyncAIPos(Message message)
    {
        AI ai = null;
        if (Singleton.ais.TryGetValue(message.GetInt(), out ai))
        {
            ai.Move(message.GetUInt(), message.GetVector3());
        }
    }
}
