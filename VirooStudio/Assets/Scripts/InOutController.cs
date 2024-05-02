using System;
using System.Collections.Generic;
using gameControllerNamespace;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UnityEngine;

namespace DefaultNamespace
{
    public class InOutController : MonoBehaviour
    { 
        public bool isSelected;
    
        [SerializeField]
        private GameController gameController;
        

        public ComponentController component;
        
        public string linkedWith = "";
        public string linkedWithIn = "";
        
        public Color originalColor;
        



        private void Start()
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    
    
        public void MarkAsSelected(string data){

            GameObject otherComponent = GameObject.Find(gameController.selectedComponent);
            
            GameObject otherInput = GameObject.Find(gameController.selectedComponent + "/Entradas/" + gameController.selectedInput);
            //GameObject otherInput = GameObject.Find(gameController.selectedInput);

            if (!gameController.players.TryGetValue(data, out GameObject[] selectedInputs))
            {
                gameController.players.Add(data, new GameObject[]{this.gameObject, null});
            }
            

           
            //Se selecciona el que ya esta seleccionado
            if(isSelected){
               isSelected = false;
               gameController.selectedComponent = "";
               gameController.selectedInput = "";
               //GetComponent<Renderer>().material.color = originalColor;
               return; 
            }
            
            //Desvinculacion de entradas
            if (linkedWith != "" && otherComponent != null)
            {
                Debug.Log("DESVINCULO");
                linkedWith = "";
                linkedWithIn = "";
                
                isSelected = false;
                //component.isSelected = false;
                //otherComponent.GetComponent<ComponentController>().isSelected = false;
                otherInput.GetComponent<InOutController>().isSelected = false;

                
                gameController.selectedComponent = "";
                gameController.selectedInput = "";
                
                //GetComponent<Renderer>().material.color = originalColor;
                //otherComponent.GetComponent<Renderer>().material.color = otherComponent.GetComponent<ComponentController>().originalColor;
                
                //transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                //otherInput.transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                
                return;
            }
            
            //Link de 2 entradas
            if(gameController.selectedInput != ""){
                linkedWith = otherComponent.GetComponent<ComponentController>().componentSO.componentName;
                linkedWithIn = otherInput.name;

                otherInput.GetComponent<InOutController>().linkedWith = component.name;
                otherInput.GetComponent<InOutController>().linkedWithIn = name;

                isSelected = false;
                //component.isSelected = false;
                //otherComponent.GetComponent<ComponentController>().isSelected = false;
                Debug.Log(otherComponent.GetComponent<ComponentController>().componentName);
                
                gameController.selectedComponent = "";
                gameController.selectedInput = "";
                
                //Color color = Random.ColorHSV();
                //GetComponent<Renderer>().material.color = color;
                //otherComponent.GetComponent<Renderer>().material.color = color;
                
                //transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                //otherInput.transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                gameController.DrawLine(transform, otherInput.transform); 
                
                
                return;
            }
            
            isSelected = true;
            
            gameController.selectedComponent = component.name;
            gameController.selectedInput = name;


        }
        
        
    }
    
    
}