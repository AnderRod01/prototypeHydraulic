using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using gameControllerNamespace;
using Serilog;
using UnityEngine;
using Viroo.Interactions;

namespace VirooLab.Actions
{
    public class CheckAction : BroadcastObjectAction
    {

        [SerializeField] private GameController gameController;

        [SerializeField] private InOutController inputPrueba;
        
        protected override void LocalExecuteImplementation(string data)
        {
            if (CheckSolution())
            {
                gameController.StartCoroutine(gameController.ShowCorrect());
                return;
            }

            gameController.StartCoroutine(gameController.ShowIncorrect());
        }

        private bool CheckSolution()
        {
            GameObject[] usedComponents = GameObject.FindGameObjectsWithTag("Respawn");
            
            //TODO: Pruebas con LineRenderer y Tags en SinglePlayer de Viroo
            

            foreach (GameObject component in usedComponents)
            {
                
                List<GameObject> inputs = FindChildrenWithTag(component, "Finish");
                foreach (GameObject input in inputs)
                {
                    //Debug.Log(input.GetComponent<InOutController>().linkedWith);
                    //Debug.Log(input.GetComponent<InOutController>().component.GetComponent<ComponentController>().componentName);

                    
                    string linkedWithIn = input.GetComponent<InOutController>().linkedWithIn;
                    string linkedWith = input.GetComponent<InOutController>().linkedWith;
                    
                    
                    if (!gameController.solution.ContainsKey(component.name + " - " + input.name))
                    {
                        Debug.Log(component.name + " - " + input.name);
                        Debug.Log(linkedWith + " - " + linkedWithIn);
                        return false;
                    }
                    
                    string value = gameController.solution[component.name + " - " + input.name];
                    if (value != linkedWith + " - " + linkedWithIn)
                    {
                        Debug.Log(component.name + " - " + input.name);
                        Debug.Log(linkedWith + " - " + linkedWithIn);
                        return false;
                    }
                }
            }
            gameController.StartCoroutine(gameController.ShowCorrect());
            return true;
        }

        public static List<GameObject> FindChildrenWithTag(GameObject parent, string tag)
        {
            List<GameObject> children = new List<GameObject>();
 
            foreach(Transform transform in parent.transform) {
                if(transform.CompareTag(tag)) {
                    children.Add(transform.gameObject);
                    break;
                }
            }
            return children;
        }
    }
}
