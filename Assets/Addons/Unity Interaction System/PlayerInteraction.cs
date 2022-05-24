using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    public GameObject InteractionManager;

    public bool canInteract = false;
    public bool isTriggered = false;
    public bool playOnce = true;

    //Hide interaction UI on start
    void Start()
    {
        interactionUI.SetActive(false);
    }

    //Detect whether the player is in range to interact
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            
            //Enable the player to interact if they have not already interacted
            if (isTriggered == false && playOnce == true)
            {
                IInteractable interactable = InteractionManager.GetComponent<IInteractable>();

                interactionText.text = interactable.GetDescription();

                canInteract = true;

                interactionUI.SetActive(canInteract);
            }

            //Enable the player to interact regardless of how many times they have allready interacted
            if (playOnce == false)
            {
                IInteractable interactable = InteractionManager.GetComponent<IInteractable>();

                interactionText.text = interactable.GetDescription();

                canInteract = true;

                interactionUI.SetActive(canInteract);
            }

        }
    }

    //Disable the oppourtunity to interact and hide interaction UI as the player is out of range
    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
        interactionUI.SetActive(false);
    }

    //Check if the player has started the interaction and run the interaction logic on the relevant script
    void Update()
    {
        if (canInteract == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractable interactable = InteractionManager.GetComponent<IInteractable>();
                interactable.Interact();
                isTriggered = true;
                interactionUI.SetActive(false);
                canInteract = false;
            }
        }
    }
}
