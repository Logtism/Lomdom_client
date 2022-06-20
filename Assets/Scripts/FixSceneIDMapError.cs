using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FixSceneIDMapError : MonoBehaviour
{
    [MenuItem("Tools/SceneIDMap Fixer")]
    public static void KillSceneIdMap()
    {
        var obj = GameObject.Find("SceneIDMap");
        if (obj != null)
        {
            DestroyImmediate(obj);
            Debug.Log("Cleared a SceneIDMap instance");
        }
    }
}