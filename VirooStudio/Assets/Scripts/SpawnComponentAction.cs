using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Viroo.Interactions;
using Viroo.UI;

namespace VirooLab.Actions
{
    public class SpawnComponentAction : BroadcastObjectAction
    {
        [SerializeField] private GameObject manometro;
        [SerializeField] private Transform spawnPoint;
        
        protected override void LocalExecuteImplementation(string data)
        {
            GameObject spawned = Instantiate(manometro);
            spawned.transform.SetParent(GameObject.Find("Root").transform);
        }
    }
}

