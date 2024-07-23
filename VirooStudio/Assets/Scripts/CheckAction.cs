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
            
            List<string[]> solution = new List<string[]>();
            gameController.solution.ForEach(solution.Add);
            
            GameObject[] usedComponents = GameObject.FindGameObjectsWithTag("Respawn");

            int aciertosNecesarios = 0;
            
            Debug.Log(usedComponents.Length);
            foreach (GameObject component in usedComponents)
            {
                
                List<GameObject> inputs = FindChildrenWithTag(BuscarEntradaEnFBX(component.transform), "Finish");
                foreach (GameObject input in inputs)
                {
                    string linkedWithIn = input.GetComponent<InOutController>().linkedWithIn;
                    string linkedWith = input.GetComponent<InOutController>().linkedWith;

                   

                    string[] goodConnection = null;
                    foreach (var connection in solution)
                    {
                        if (connection[0].Equals(component.GetComponent<ComponentController>().componentName + " - " + input.name))
                        {

                            if (connection[1].Equals(linkedWith + " - " + linkedWithIn))
                            {

                                goodConnection = connection;
                            }
                        }
                    }
                    solution.Remove(goodConnection);
                }
            }

            Debug.Log("La cantidad restante de soluciones son: " + solution.Count);
            if (solution.Count == 0)
                return true;
            return false;
        }

        public static List<GameObject> FindChildrenWithTag(GameObject parent, string tag)
        {
            List<GameObject> children = new List<GameObject>();
            //Debug.Log(parent.name + " ----- " + parent.transform.parent.name);
            
            foreach(Transform child in parent.transform) {
                Debug.Log(child.name);
                
                if(child.CompareTag(tag)) {
                    children.Add(child.gameObject);
                }
            }
            return children;
        }
        
        GameObject BuscarEntradaEnFBX(Transform parent)
        {
            Transform[] children = parent.GetComponentsInChildren<Transform>(false);

            foreach (Transform child in children)
            {

                if (child.gameObject.activeSelf && child.name.Equals("Entradas"))
                {
                    return child.gameObject;
                }
            }

            return null;
        }
    }
}
