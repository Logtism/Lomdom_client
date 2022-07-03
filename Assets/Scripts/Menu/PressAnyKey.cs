using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
    public GameObject mainLoginPanel;
    public bool hasPressedKey;

    public void Update()
    {
        if (hasPressedKey == false)
        {
            if (Input.anyKeyDown)
            {
                mainLoginPanel.SetActive(true);
                this.gameObject.SetActive(false);
                hasPressedKey = true;
                return;
            }
        }
    }
}
