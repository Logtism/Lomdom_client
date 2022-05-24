using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineOnInteract : MonoBehaviour, IInteractable
{
    
    public PlayableDirector timeline;

    public string GetDescription()
    {
        return "Interact";
    }

    public void Interact()
    {
        timeline.Play();
    }
}
