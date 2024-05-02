using System;
using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        componentName = componentSO.componentName;
        socket = GetComponent<XRSocketInteractor>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //NetworkObject.GenerateRandomId(SceneManager.GetActiveScene());
    }
    
}
