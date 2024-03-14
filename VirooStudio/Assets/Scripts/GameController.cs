using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Community.Libraries.Humility;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public string selectedComponent = "";

    public GameObject[] availableComponents;

    public Dictionary<string, string> solution = new Dictionary<string, string>();


    private void Start()
    {
         availableComponents = GameObject.FindGameObjectsWithTag("Component");
         

        solution.Add("Component_A", "Component_B");
        solution.Add("Component_B", "Component_A");
        solution.Add("Component_C", "Component_D");
        solution.Add("Component_D", "Component_C");

        solution.TryGetValue("Component_A", out string value);

    }
}
