using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dialogType
{
    Confirm,
    Message,
}


[CreateAssetMenu(fileName = "New popUp", menuName = "DialogBox")]
public class PopUp : ScriptableObject
{
    [SerializeField] public dialogType dialogType; 
    [SerializeField] public string DialogTitle;
    [SerializeField] public string DialogMsg;
    [SerializeField] public string ConfirmButtonText;
    [SerializeField] public Color32 DialogMsgColor;

    [Header("Dialog awknoledgement (required)")]
    [SerializeField] public Confirm OnConfirmFunction = new Confirm();

    [Header("Dialog Deny (conformation dialog only)")]
    [SerializeField] public Deny OnDialogDenyFunction = new Deny();
    [SerializeField] public string DenyButtonText;
}
