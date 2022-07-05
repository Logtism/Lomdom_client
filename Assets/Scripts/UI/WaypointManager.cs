using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using TMPro;
using UnityEngine.UI;

public class WaypointManager : MonoBehaviour
{
    private static WaypointManager _singleton;
    public static WaypointManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(WaypointManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private GameObject waypointMarker;
    private Image waypointMarkerImage;

    [Header("Waypoint settings")]
    [SerializeField] private Vector3 waypointOffset;
    [SerializeField] private GameObject waypointHolder;
    private Transform selectedWaypoint;
    private Camera mainCam;
    private bool isWaypointActive = false;
    private GameObject localPlayer;
    private int distanceToSelected;

    private void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        waypointMarkerImage = waypointMarker.GetComponent<Image>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        waypointHolder = GameObject.FindGameObjectWithTag("waypointHolder");
        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
    }

    void Update()
    {
        if(isWaypointActive == true)
        {
            Vector3 pos = mainCam.WorldToScreenPoint(selectedWaypoint.position + waypointOffset);
            if(pos.z < 0) { pos *= -1; }

            float dist = Vector3.Distance(waypointHolder.transform.position, localPlayer.transform.position);
            distanceToSelected = Mathf.RoundToInt(dist);
            distanceText.text = distanceToSelected.ToString();

            if (waypointMarker.transform.position != pos)
                waypointMarker.transform.position = pos;
        }
    }

    [MessageHandler((ushort)Messages.STC.waypointUpdate)]
    private static void OnWaypointUpdated(Message message)
    {
        Singleton.startWaypoint(message.GetVector3());
    }

    public void startWaypoint(Vector3 waypointLocation)
    {
        if (isWaypointActive == false)
        {
            Debug.Log("Instantiated waypoint at " + waypointLocation);
            waypointHolder.transform.position = waypointLocation;
            selectedWaypoint = waypointHolder.transform;
            distanceText.gameObject.SetActive(true);
            waypointMarker.SetActive(true);
            isWaypointActive = true;
        }

        else
        {
            waypointHolder.transform.position = waypointLocation;
            selectedWaypoint = waypointHolder.transform;
        }
    }

    public void endWaypoint()
    {
        distanceText.gameObject.SetActive(false);
        waypointMarker.SetActive(false);
        isWaypointActive = false;
    }
}
