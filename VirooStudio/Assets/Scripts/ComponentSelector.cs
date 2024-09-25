using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComponentSelector : MonoBehaviour
{
    public GameObject[] components;
    public GameObject[] manometros;
    public GameObject[] estranguladoras;

    
    
    public int currentIndex;
    public int currentIndexManometro, currentIndexestranguladora;
    

    [SerializeField] private TextMeshProUGUI currentComponentName;
    
    public Button nextButton;
    public Button prevButton;
    public Button spawnButton;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        currentIndexManometro = 0;
        currentIndexestranguladora = 0;
        UpdateComponent();
        UpdateButtonInteractable();

    }
    
    // Método para actualizar el texto del componente actual
    void UpdateComponent()
    {
        
        
        for (int i = 0; i < components.Length; i++)
        {
            components[i].SetActive(i == currentIndex);
        }
        currentComponentName.text = components[currentIndex].GetComponent<ComponentController>().componentName;
    }
    
    // Método para habilitar/deshabilitar los botones según el índice actual
    void UpdateButtonInteractable()
    {
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < components.Length - 1;
    }
    
    // Método para avanzar al siguiente nivel
    public void NextComponent()
    {
        Debug.Log("BB");
        if (currentIndex < components.Length - 1)
        {
            currentIndex++;
            UpdateComponent();
            UpdateButtonInteractable();
        }
        
        
        if ((currentIndex == 4 && currentIndexestranguladora == estranguladoras.Length - 1) ||
            (currentIndex == 5 && currentIndexManometro ==  manometros.Length))
        {
            spawnButton.interactable = false;
            spawnButton.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            spawnButton.interactable = true;
            spawnButton.GetComponent<BoxCollider>().enabled = true;
        }

    }

    // Método para retroceder al nivel anterior
    public void PreviousComponent()
    {
        Debug.Log("AA");
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateComponent();
            UpdateButtonInteractable();
        }
        
      if ((currentIndex == 4 && currentIndexestranguladora == estranguladoras.Length - 1) ||
          (currentIndex == 5 && currentIndexManometro ==  manometros.Length))
      {
          spawnButton.interactable = false;
          spawnButton.GetComponent<BoxCollider>().enabled = false;
      }
      else
      {
          spawnButton.interactable = true;
          spawnButton.GetComponent<BoxCollider>().enabled = true;
      }
    }
    
    
}
