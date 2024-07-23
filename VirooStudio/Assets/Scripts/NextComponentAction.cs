using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viroo.Interactions;

public class NextComponentAction : BroadcastObjectAction
{
    [SerializeField] private ComponentSelector componentSelector;
    protected override void LocalExecuteImplementation(string data)
    {
        componentSelector.NextComponent();
    }
}