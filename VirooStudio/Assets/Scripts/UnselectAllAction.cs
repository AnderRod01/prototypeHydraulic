using System.Collections;
using System.Collections.Generic;
using gameControllerNamespace;
using UnityEngine;
using Viroo.Interactions;

public class UnselectAllAction : BroadcastObjectAction
{
    [SerializeField] private GameController gameController;
    protected override void LocalExecuteImplementation(string data)
    {
        gameController.UnselectAll();
    }
}
