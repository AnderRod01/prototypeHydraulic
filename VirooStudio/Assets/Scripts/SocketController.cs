using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketController : MonoBehaviour
{
    private XRSocketInteractor socket;
    private XRBaseInteractable attachedObject;
    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(EnterSnap);
        socket.selectExited.AddListener(ExitSnap);
    }
    
    public void EnterSnap(SelectEnterEventArgs args)
    {
        attachedObject = args.interactable;
        Debug.Log("Objeto enganchado: " + attachedObject.gameObject.name);
        BoxCollider[] colliders = attachedObject.GetComponentsInChildren<BoxCollider>();

        for (int i = 1; i < colliders.Length; i++)
        {
            BoxCollider collider = colliders[i];
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
        BoxCollider[] colliders = attachedObject.GetComponentsInChildren<BoxCollider>();

        for (int i = 1; i < colliders.Length; i++)
        {
            BoxCollider collider = colliders[i];
            collider.enabled = false;
            Debug.Log(collider.gameObject.name);
        }
    }
}
