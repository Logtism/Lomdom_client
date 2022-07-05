using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using UnityEngine.UI;

public class CTS_InteractionInputHandler : MonoBehaviour
{
    private static CTS_InteractionInputHandler _singleton;
    public static CTS_InteractionInputHandler Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(CTS_InteractionInputHandler)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private bool checkInputForRobbery;
    [SerializeField] private float robberyHoldTime;
    private float robberyTimer;
    private Image Interaction_Fill;
    private GameObject Interaction_Frame;

    private void Awake()
    {
        Singleton = this;

        robberyTimer = robberyHoldTime;
    }

    [MessageHandler((ushort)Messages.STC.playerEnter_RobberyTrigger)]
    private static void OnPlayerEnterRobberyTrigger(Message message)
    {
        Singleton.showInteractionUI();
        
    }

    [MessageHandler((ushort)Messages.STC.playerExit_RobberyTrigger)]
    private static void OnPlayerExitRobberyTrigger(Message message)
    {
        Singleton.checkInputForRobbery = false;
        Singleton.hideInteractionUI();
    }

    private void Update()
    {
        if (checkInputForRobbery)
        {
            Interaction_Fill.fillAmount = robberyTimer / 5;
            
            if (Input.GetKey(KeyCode.E))
            {
                robberyTimer -= Time.deltaTime;
                Debug.Log(robberyTimer);
            }
            else
                robberyTimer = robberyHoldTime;

            if (robberyTimer < 0)
            {
                Debug.Log("completeTimer");
                checkInputForRobbery = false;

                hideInteractionUI();

                Message message = Message.Create(MessageSendMode.reliable, Messages.CTS.completeRobbery_input);
                NetworkManager.Singleton.Client.Send(message);
            }
        }
    }

    private void showInteractionUI()
    {
        Interaction_Frame = GameObject.FindGameObjectWithTag("Interaction_Frame");
        
        for (int a = 0; a < Interaction_Frame.transform.childCount; a++)
        {
            Interaction_Frame.transform.GetChild(a).gameObject.SetActive(true);
        }

        Interaction_Fill = GameObject.FindGameObjectWithTag("Interaction_Fill").GetComponent<Image>();       
        checkInputForRobbery = true;
    }

    private void hideInteractionUI()
    {
        for (int a = 0; a < Interaction_Frame.transform.childCount; a++)
        {
            Interaction_Frame.transform.GetChild(a).gameObject.SetActive(false);
        }
    }
}
