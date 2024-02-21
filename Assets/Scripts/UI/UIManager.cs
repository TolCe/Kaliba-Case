using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LevelSuccessPanel _levelSuccessPanel;

    private void OnEnable()
    {
        BusSystem.Level.OnLevelLoad += OnLevelLoaded;
        BusSystem.Level.OnLevelSuccess += OnLevelSuccess;
    }

    private void OnDisable()
    {
        BusSystem.Level.OnLevelLoad -= OnLevelLoaded;
        BusSystem.Level.OnLevelSuccess -= OnLevelSuccess;
    }

    private void OnLevelLoaded()
    {
        _levelSuccessPanel.Hide();
    }

    private void OnLevelSuccess()
    {
        _levelSuccessPanel.Show();
    }
}
