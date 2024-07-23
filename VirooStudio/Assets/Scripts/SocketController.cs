using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketController : MonoBehaviour
{
    private XRBaseInteractable attachedObject;

    public void EnterSnap(SelectEnterEventArgs args)
    {
        attachedObject = args.interactable;
        //Debug.Log("Objeto enganchado: " + attachedObject.gameObject.name);
        GameObject entrada = BuscarEntradaEnFBX(attachedObject.transform);
        BoxCollider[] colliders = entrada.gameObject.GetComponentsInChildren<BoxCollider>();

        BoxCollider inputT = null;
        
        foreach (BoxCollider collider in colliders)
        {

            collider.enabled = true;
            Debug.Log(collider.gameObject.name);
        }

        //IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
        //Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

    public void ExitSnap(SelectExitEventArgs args)
    {
        attachedObject = args.interactable;
        //Debug.Log("Objeto enganchado: " + attachedObject.gameObject.name);
        GameObject entrada = BuscarEntradaEnFBX(attachedObject.transform);
        BoxCollider[] colliders = entrada.gameObject.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
            Debug.Log(collider.gameObject.name);
        }
        /*for (int i = 1; i < colliders.Length; i++)
        {
            BoxCollider collider = colliders[i];
            collider.enabled = false;
            
        }*/
    }

    GameObject BuscarEntradaEnFBX(Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(false);

        foreach (Transform child in children)
        {
            Debug.Log(child.name);

            if (child.gameObject.activeSelf && child.name.Equals("Entradas"))
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
