using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Interpolator interpolator;

    public int id;
    private Vector3 Waypoint;
    private bool GoingToWaypoint;
    private Vector3 Point;
    public AIMode ai_mode;

    private void Start()
    {
        agent.autoBraking = false;
    }

    public void SetInfoWaypoint(int id, Vector3 Waypoint)
    {
        ai_mode = AIMode.waypoint;
        this.id = id;
        this.Waypoint = Waypoint;
    }

    public void SetInfoPatrol(int id, Vector3 Point)
    {
        ai_mode = AIMode.patrol;
        this.id = id;
        this.Point = Point;
    }

    public void Move(uint tick, Vector3 NewPosition)
    {
        interpolator.NewUpdate(tick, NewPosition);
    }
}
