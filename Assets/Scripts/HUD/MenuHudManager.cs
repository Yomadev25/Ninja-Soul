using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHudManager : MonoBehaviour
{
    [Header("Title HUD")]
    [SerializeField]
    private TMP_Text _versionText;

    void Start()
    {
        _versionText.text = $"v {Application.version}";
    }

    void Update()
    {
        
    }
}
