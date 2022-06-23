using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButtonObject> tabButtons;

    public Color32 tabIdle;
    public Color32 tabHover;
    public Color32 tabActive;
    public TabButtonObject selectedTab;
    public List<GameObject> tabStates;
    
    public void Subscribe(TabButtonObject button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButtonObject>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtonObject button) 
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.image.color = tabHover;
        }
    }

    public void OnTabExit(TabButtonObject button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonObject button)
    {
        selectedTab = button;
        ResetTabs();
        button.image.color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<tabStates.Count; i++)
        {
            if (i == index)
            {
                tabStates[i].SetActive(true);
            }
            else
            {
                tabStates[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabButtonObject button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.image.color = tabIdle;
        }
    }

    private void Update()
    {
        
    }

}
