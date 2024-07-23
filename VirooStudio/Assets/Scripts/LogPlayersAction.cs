using System.Collections;
using System.Collections.Generic;
using gameControllerNamespace;
using UnityEngine;
using Viroo.Interactions;

namespace VirooLab.Actions
{
    public class LogPlayersAction : BroadcastObjectAction
    {
        [SerializeField] private GameController gameController;

        protected override void LocalExecuteImplementation(string data)
        {
            int i = 1;
            /*foreach (var value in gameController.players.Keys)
            {
                Debug.Log("Jugador numero " + i + ": " + value); 
            }

            Debug.Log("Length: " + gameController.players.Count);*/
        }
        
    }
}
