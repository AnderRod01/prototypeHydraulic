using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viroo.Interactions;

public class PrevComponentAction : BroadcastObjectAction
{
    [SerializeField] private ComponentSelector componentSelector;
    protected override void LocalExecuteImplementation(string data)
    {
        componentSelector.PreviousComponent();
    }
}