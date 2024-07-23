using System;
using System.Collections;
using System.Collections.Generic;
using gameControllerNamespace;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UnityEngine;
using Viroo.Networking;

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
        
        public Material originalColor;
        public Color selectedColor;

        public string selectionPlayer;
        
        private void Start()
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
            
        }
    
    
        public void MarkAsSelected(string data){

            Debug.Log(data);
            GameObject otherComponent = null;
            if(gameController.players.ContainsKey(data))
                otherComponent = GameObject.Find(gameController.players[data][0]);
            
            GameObject otherInput = null;
            if (otherComponent != null)
            {
                otherInput = GameObject.Find(otherComponent.name + "/" + BuscarNombreFBX(otherComponent.transform) + "/Entradas/" + gameController.players[data][1]);
            }
            
            if (!gameController.players.ContainsKey(data))
            {
                gameController.players.Add(data, new []{"", ""});
            }
            

           
            //Se selecciona el que ya esta seleccionado
            if(isSelected && selectionPlayer.Equals(data)){
                
                Debug.Log("SELECCIONA EL MISMO");
                isSelected = false;
                //gameController.selectedComponent = "";
                //gameController.selectedInput = "";
                gameController.players[data] = new[] { "", "" };
                
                selectionPlayer = "";
                //GetComponent<Renderer>().material.color = originalColor;
                StopAllCoroutines();
                changeSelectedColor();
                return; 
            }
            
            //Desvinculacion de entradas
            if (linkedWith != "" && otherComponent != null && otherInput.GetComponent<InOutController>().selectionPlayer.Equals(data))
            {
                Debug.Log("DESVINCULO");
                StopAllCoroutines();
                otherInput.GetComponent<InOutController>().StopAllCoroutines();
                
                Destroy(GameObject.Find("Cable " + transform.GetComponent<InOutController>().component.componentName + " - " + name
                + " / " + otherInput.GetComponent<InOutController>().component.componentName + " - " + otherInput.name));
                
                Destroy(GameObject.Find("Cable " + otherInput.GetComponent<InOutController>().component.componentName + " - " + otherInput.name
                                        + " / " + transform.GetComponent<InOutController>().component.componentName + " - " + name));

                
                
                linkedWith = "";
                linkedWithIn = "";
                selectionPlayer = "";
                otherInput.GetComponent<InOutController>().selectionPlayer = "";
                isSelected = false;
                //component.isSelected = false;
                //otherComponent.GetComponent<ComponentController>().isSelected = false;
                otherInput.GetComponent<InOutController>().isSelected = false;

                
                //gameController.selectedComponent = "";
                //gameController.selectedInput = "";
                gameController.players[data] = new[] { "", "" };

                
                changeSelectedColor();
                otherInput.GetComponent<InOutController>().changeSelectedColor();
                
                //GetComponent<Renderer>().material.color = originalColor;
                //otherComponent.GetComponent<Renderer>().material.color = otherComponent.GetComponent<ComponentController>().originalColor;
                
                //transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                //otherInput.transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);

                return;
            }
            
            //Link de 2 entradas
            if(gameController.players[data][0] != "" && otherInput.GetComponent<InOutController>().selectionPlayer.Equals(data)){
                Debug.Log("LINK DE 2 ENTRADAS");
                
                StopAllCoroutines();
                otherInput.GetComponent<InOutController>().StopAllCoroutines();
                
                linkedWith = otherComponent.GetComponent<ComponentController>().componentSO.componentName;
                linkedWithIn = otherInput.name;

                otherInput.GetComponent<InOutController>().linkedWith = component.componentName;
                otherInput.GetComponent<InOutController>().linkedWithIn = name;

                isSelected = false;
                otherInput.GetComponent<InOutController>().isSelected = false;
                
                //otherComponent.GetComponent<ComponentController>().isSelected = false;
                Debug.Log(otherComponent.GetComponent<ComponentController>().componentName);
                
                //gameController.selectedComponent = "";
                //gameController.selectedInput = "";
                gameController.players[data] = new[] { "", "" };
                
                component.GetComponent<ComponentController>().unableCollider();

                selectionPlayer = "";
                otherInput.GetComponent<InOutController>().selectionPlayer = "";
                changeSelectedColor();
                otherInput.GetComponent<InOutController>().changeSelectedColor();
                
                //transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                //otherInput.transform.GetChild(0).GetComponent<MeshRenderer>().transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                gameController.DrawLine(transform, otherInput.transform); 
                return;
            }
            
            isSelected = true;
            selectionPlayer = data;
            changeSelectedColor();
            //gameController.selectedComponent = component.name;
            //gameController.selectedInput = name;

            gameController.players[data] = new[] { component.name, name };
            
            StartCoroutine(UnselectTimer(data));
        }

        private IEnumerator UnselectTimer(string data)
        {
            if (isSelected)
            {
                Debug.Log("preparo para desvincular");
                yield return new WaitForSeconds(10f);
            
                isSelected = false;
                //gameController.selectedComponent = "";
                //gameController.selectedInput = "";
                
                gameController.players[data] = new[] { "", "" };

                changeSelectedColor();
            
                Debug.Log("desvinculado");

            }
        }
        
        private string BuscarNombreFBX(Transform padre)
        {
            foreach (Transform hijo in padre)
            {
                if (hijo.gameObject.activeSelf)
                {
                    return hijo.name; // Devuelve el nombre del primer hijo habilitado que encuentres
                }
            }
            return null; // Devuelve null si no se encuentra ningún hijo habilitado
        }

        private void changeSelectedColor()
        {
            if (isSelected)
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();

                // Recorrer cada Renderer y hacer algo con ellos
                foreach (Renderer renderer in renderers)
                {
                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    renderer.material.SetColor("_Color", selectedColor);
                }
            }
            else
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();

                // Recorrer cada Renderer y hacer algo con ellos
                foreach (Renderer renderer in renderers)
                {
                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    renderer.material = originalColor;
                }
            }
        }
        
    }
    
    
    
    
}