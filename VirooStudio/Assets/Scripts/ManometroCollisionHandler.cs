using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = UnityEngine.Windows.Input;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;
using System.Linq;


public class ManometroCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameObject manometro; 
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject valvulaEstranguladora;
    [SerializeField] private GameObject inputT;
    [SerializeField] private GameObject inputT1;
    private void Start()
    {
        root = GameObject.Find("Root");
 
    }


    public void HandleCollision()
    {
        
        manometro.transform.parent = root.transform;
        
        valvulaEstranguladora.SetActive(true);
        inputT1.SetActive(true);

        inputT.GetComponent<BoxCollider>().enabled = false;

    }

    private IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
    }
}
