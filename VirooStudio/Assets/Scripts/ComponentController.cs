using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentController : MonoBehaviour
{
    
    public bool isSelected;

    public string linkedWith;

    [SerializeField] 
    private ComponentScriptableObject componentSO;
    
    [SerializeField]
    private GameController gameController;


    public void MarkAsSelected(){
        
        
        if(isSelected){
           isSelected = false;
           gameController.selectedComponent = "";
           return; 
        }
        
        if(gameController.selectedComponent != ""){
            GameObject otherComponent = GameObject.Find(gameController.selectedComponent);
            otherComponent.GetComponent<ComponentController>().linkedWith = componentSO.componentName;
            linkedWith = otherComponent.GetComponent<ComponentController>().componentSO.componentName;

            isSelected = false;
            gameController.selectedComponent = "";
            
            Debug.Log(linkedWith + " linked with " + otherComponent.GetComponent<ComponentController>().linkedWith);
            return;
        }
        
        isSelected = true;
        gameController.selectedComponent = componentSO.componentName;
    }
}
