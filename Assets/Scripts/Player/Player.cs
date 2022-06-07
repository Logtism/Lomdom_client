using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform CamTransform;
    [SerializeField] private Interpolator interpolator;

    public bool IsLocal;
    public void Move(uint tick, Vector3 NewPosition, Vector3 forward)
    {
        interpolator.NewUpdate(tick, NewPosition);

        if (IsLocal == false)
        {
            // Used for rotation but its a bit fucked so yeah.
            // CamTransform.forward = forward;
        }
    }
}
