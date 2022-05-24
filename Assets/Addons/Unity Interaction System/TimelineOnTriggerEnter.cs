using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineOnTriggerEnter : MonoBehaviour
{
    
    public PlayableDirector timeline;
    public bool playonce = true;
    public bool IsTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (IsTriggered == false && playonce == true)
        {
            timeline.Play();
            IsTriggered = true;
        }

        if (playonce == false)
        {
            timeline.Play();
            IsTriggered = true;
        }
    }
}
