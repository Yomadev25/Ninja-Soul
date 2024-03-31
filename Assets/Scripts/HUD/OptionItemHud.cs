using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionItemHud : MonoBehaviour
{
    public const string MessageWantToAdjustOption = "Want To Adjust Option";

    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Button _backButton;

    [Header("Value Icon")]
    [SerializeField]
    private Image[] _valueIcons;
    [SerializeField]
    private Sprite[] _valueSprites;

    [Header("Option Text")]
    [SerializeField]
    private TMP_Text _optionText;

    private void Start()
    {
        _nextButton.onClick.AddListener(() => AdjustOption(true));
        _backButton.onClick.AddListener(() => AdjustOption(false));
    }

    private void AdjustOption(bool isNext)
    {
        MessagingCenter.Send(this, MessageWantToAdjustOption, isNext);
    }

    public void UpdateOptionValue(int value)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i >= value)
            {
                _valueIcons[i].sprite = _valueSprites[0];
            }
            else
            {
                _valueIcons[i].sprite = _valueSprites[1];
            }
        }
    }

    public void UpdateOptionValue(string value)
    {
        _optionText.text = value;
    }
}
