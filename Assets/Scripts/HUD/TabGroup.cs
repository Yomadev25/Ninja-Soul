using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private List<TabButton> _tabButtons = new List<TabButton>();
    [SerializeField]
    private GameObject[] _panels;

    [Header("State Sprites")]
    [SerializeField]
    private Sprite _idleSprite;
    [SerializeField]
    private Sprite _hoverSprite;
    [SerializeField]
    private Sprite _activeSprite;

    private TabButton _selectedButton;

    public void SubscribeTabButton(TabButton button)
    {
        if (!_tabButtons.Contains(button))
            _tabButtons.Add(button);
    }

    public void UnsubscribeTabButton(TabButton button)
    {
        if (_tabButtons.Contains(button))
            _tabButtons.Remove(button);
    }

    private void ResetTabs()
    {
        foreach (TabButton button in _tabButtons)
        {
            if (_selectedButton == null || button != _selectedButton)
            {
                button.SetBackground(_idleSprite);
            }         
        }
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        button.SetBackground(_hoverSprite);
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (_selectedButton != null)
        {
            button.Deselect();
        }

        _selectedButton = button;
        _selectedButton.Select();
        
        ResetTabs();

        button.SetBackground(_activeSprite);

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == index)
            {
                _panels[i].SetActive(true);
            }
            else
            {
                _panels[i].SetActive(false);
            }
        }
    }
}
