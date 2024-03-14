using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viroo.Interactions;

namespace VirooLab.Actions
{
    public class CheckController : BroadcastObjectAction
    {
		
		[SerializeField] private GameController gameController;
        
        protected override void LocalExecuteImplementation(string data)
        {
            Debug.Log("Button Check");
            CheckSolution();
        }

        private void CheckSolution()
        {
            GameObject[] usedComponents = GameObject.FindGameObjectsWithTag("Component");
            
            foreach (GameObject component in usedComponents)
            {
                string linkedWith = component.GetComponent<ComponentController>().linkedWith;
                if (linkedWith != gameController.solution.TryGetValue(component.GetComponent<ComponentController>().componentName, out string value))
                {
                    Debug.Log("not correct");
                    return;
                }
            }
        }
    }
}
