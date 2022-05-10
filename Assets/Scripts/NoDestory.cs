using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestory : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
