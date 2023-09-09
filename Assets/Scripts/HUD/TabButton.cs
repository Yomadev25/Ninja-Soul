using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private TabGroup _tabGroup;
    [SerializeField]
    private bool _isDefault;

    [SerializeField]
    private Image _background;

    [SerializeField]
    private UnityEvent onSelected;
    [SerializeField]
    private UnityEvent onDeselected;

    void Start()
    {
        if (_background == null)
        {
            Debug.LogErrorFormat("{0} doesn't has {1}", nameof(Image), this.name);
        }

        if ( _tabGroup == null)
        {
            Debug.LogErrorFormat("{0} doesn't has {1}", nameof(TabGroup), this.name);
        }
        else
        {
            _tabGroup.SubscribeTabButton(this);
            if (_isDefault)
            {
                _tabGroup.OnTabSelected(this);
        }
        }
    }

    private void OnDestroy()
    {
        _tabGroup.UnsubscribeTabButton(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this);
    }

    public void SetBackground(Sprite sprite)
    {
        _background.sprite = sprite;
    }

    public void Select()
    {
        onSelected?.Invoke();
    }

    public void Deselect()
    {
        onDeselected?.Invoke();
    }
}
