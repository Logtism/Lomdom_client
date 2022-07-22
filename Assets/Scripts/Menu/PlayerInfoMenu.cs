using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoMenu : MonoBehaviour
{
    private static PlayerInfoMenu _singleton;
    public static PlayerInfoMenu Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(PlayerInfoMenu)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private TextMeshProUGUI[] PlayerNameText;
    [SerializeField] private TextMeshProUGUI[] PlayerLevelText;
    [SerializeField] private TextMeshProUGUI[] PlayerMoneyText;

    private void Awake()
    {
        Singleton = this;
    }

    private void updatePlayerInfo(string name, string balance, string level)
    {
        for (int i = 0; i < PlayerNameText.Length; i++) { PlayerNameText[i].text = name; }
        for (int i = 0; i < PlayerLevelText.Length; i++) { PlayerLevelText[i].text = "LVL: " + level; }
        for (int i = 0; i < PlayerMoneyText.Length; i++) { PlayerMoneyText[i].text = "$" + balance; }
    }
}
