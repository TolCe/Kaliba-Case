using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public void SetParameter(Animator animator, string parameter, bool state)
    {
        animator.SetBool(parameter, state);
    }

    public void ToggleParameter(Animator animator, string parameter)
    {
        animator.SetTrigger(parameter);
    }
}
