using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAnimation : Animation
{
    [SerializeField] private Animator _animator;

    [SerializeField] private string _selectionParameterName;
    [SerializeField] private string _runParameterName;
    [SerializeField] private string _vehicleEnterParameterName;

    public void ToggleSelected(bool state)
    {
        SetParameter(_animator, _runParameterName, false);
        SetParameter(_animator, _selectionParameterName, state);
    }

    public void ToggleRun(bool state)
    {
        SetParameter(_animator, _selectionParameterName, !state);
        SetParameter(_animator, _runParameterName, state);
    }

    public void VehicleEnter()
    {
        SetParameter(_animator, _selectionParameterName, false);
        SetParameter(_animator, _runParameterName, false);
        ToggleParameter(_animator, _vehicleEnterParameterName);
    }
}
