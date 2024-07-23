using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using gameControllerNamespace;
using Unity.VisualScripting.Community.Libraries.Humility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Virtualware.Networking.Client;
using Random = UnityEngine.Random;

public class ComponentController : MonoBehaviour
{

    public string linkedWith = "";
    public string componentName;
    
    [SerializeField] public ComponentScriptableObject componentSO;
    
    [SerializeField]
    private GameController gameController;

    public Color originalColor;

    private XRSocketInteractor socket;
    private List<GameObject> inputs;
    private bool hasConnection;
    private void Start()
    {
        componentName = componentSO.componentName;
        socket = GetComponent<XRSocketInteractor>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        inputs = FindChildrenWithTag(BuscarEntradaEnFBX(this.transform), "Finish");
        //NetworkObject.GenerateRandomId(SceneManager.GetActiveScene());
    }

    private void Update()
    {
        hasConnection = false;
        foreach (GameObject input in inputs)
        {
            if (!input.GetComponent<InOutController>().linkedWith.Equals(""))
            {
                hasConnection = true;
            }
        }

        if (hasConnection)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void enableCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    public void unableCollider()
    {
        GetComponent<BoxCollider>().enabled = false;
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
