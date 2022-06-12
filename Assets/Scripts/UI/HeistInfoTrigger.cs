using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeistInfoTrigger : MonoBehaviour
{
    //public Heist heist;

    private GameObject HeistInfoManager;

    private void Awake()
    {
        HeistInfoManager = GameObject.FindWithTag("HeistInfoManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LocalPlayer")
        {
            if (HeistInfoManager.GetComponent<HeistInfoManager>().heistInfoVisible == false)
            {
                //HeistInfoManager.GetComponent<HeistInfoManager>().heist = heist;
                HeistInfoManager.GetComponent<HeistInfoManager>().showHeistInfo();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LocalPlayer")
        {
            if (HeistInfoManager.GetComponent<HeistInfoManager>().heistInfoVisible == true)
            {
                //HeistInfoManager.GetComponent<HeistInfoManager>().heist = null;
                HeistInfoManager.GetComponent<HeistInfoManager>().hideHeistInfo();
            }
        }
    }
}
