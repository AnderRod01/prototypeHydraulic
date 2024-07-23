using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ChangeColorOnHover : MonoBehaviour
{
    [SerializeField] private GameObject input;
    [SerializeField] private Material originalColor;
    [SerializeField] private Color hoverColor;

    private void Start()
    {
        var cubeRenderer = input.GetComponent<Renderer>();
        
    }

    public void EnterHover()
    {
        // Get the Renderer component from the new cube
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // Recorrer cada Renderer y hacer algo con ellos
        foreach (Renderer renderer in renderers)
        {
            // Call SetColor using the shader property name "_Color" and setting the color to red
            renderer.material.SetColor("_Color", hoverColor);
        }

    }

    public void ExitHover()
    {
        // Get the Renderer component from the new cube
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        
        if(input.GetComponent<InOutController>().isSelected)
            return;
        
        // Recorrer cada Renderer y hacer algo con ellos
        foreach (Renderer renderer in renderers)
        {
            // Call SetColor using the shader property name "_Color" and setting the color to red
            renderer.material = originalColor;
        }

    }
}
