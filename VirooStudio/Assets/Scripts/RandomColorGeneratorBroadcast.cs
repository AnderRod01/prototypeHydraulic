using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;

public class RandomColorGeneratorBroadcast : BroadcastObjectAction
{
    [SerializeField] private TextMeshPro codeDisplayDebug; // Texto3D para mostrar el n�mero aleatorio en escena con fines de depuraci�n.
    private List<Color> customColors = new List<Color>
        {
            new Color(0.9f, 0.1f, 0.1f, 1f),  // Rojo claro
            new Color(0.1f, 0.8f, 0.3f, 1f),  // Verde menta
            new Color(0.2f, 0.4f, 0.9f, 1f),  // Azul cielo
            new Color(0.9f, 0.8f, 0.2f, 1f),  // Amarillo dorado
            new Color(0.6f, 0.2f, 0.9f, 1f),  // Púrpura
            new Color(0.1f, 0.9f, 0.9f, 1f),  // Aqua brillante
            new Color(0.9f, 0.4f, 0.3f, 1f),  // Naranja coral
            new Color(0.7f, 0.7f, 0.7f, 1f),  // Gris claro
            new Color(0.3f, 0.2f, 0.1f, 1f),  // Marrón oscuro
            new Color(0.4f, 0.5f, 0.2f, 1f),  // Verde oliva
            new Color(0.5f, 0.1f, 0.7f, 1f),  // Violeta oscuro
            new Color(0.9f, 0.6f, 0.1f, 1f),  // Naranja dorado
            new Color(0.6f, 0.8f, 0.3f, 1f),  // Verde lima
            new Color(0.2f, 0.3f, 0.7f, 1f),  // Azul profundo
            new Color(0.8f, 0.1f, 0.6f, 1f),  // Rosa fuerte
            new Color(0.3f, 0.9f, 0.4f, 1f),  // Verde primavera
            new Color(0.7f, 0.2f, 0.5f, 1f),  // Fucsia oscuro
            new Color(0.4f, 0.6f, 0.9f, 1f),  // Azul pastel
            new Color(0.6f, 0.3f, 0.1f, 1f),  // Marrón claro
            new Color(0.2f, 0.8f, 0.8f, 1f),  // Verde agua
            new Color(0.9f, 0.2f, 0.2f, 1f),  // Rojo intenso
            new Color(0.8f, 0.5f, 0.2f, 1f),  // Mostaza
            new Color(0.3f, 0.9f, 0.9f, 1f),  // Verde turquesa
            new Color(0.9f, 0.3f, 0.6f, 1f),  // Rosa coral
            new Color(0.7f, 0.1f, 0.3f, 1f),  // Rojo vino
            new Color(0.5f, 0.8f, 0.1f, 1f),  // Verde hierba
            new Color(0.1f, 0.3f, 0.7f, 1f),  // Azul marino
            new Color(0.9f, 0.5f, 0.7f, 1f),  // Rosa suave
            new Color(0.2f, 0.7f, 0.9f, 1f),  // Celeste brillante
            new Color(0.4f, 0.4f, 0.6f, 1f),  // Gris azulado
            new Color(0.8f, 0.2f, 0.3f, 1f),  // Rojo carmesí
            new Color(0.3f, 0.5f, 0.9f, 1f),  // Azul vibrante
            new Color(0.5f, 0.7f, 0.2f, 1f),  // Verde bosque
            new Color(0.7f, 0.8f, 0.4f, 1f),  // Amarillo claro
            new Color(0.9f, 0.7f, 0.4f, 1f),  // Melocotón
            new Color(0.4f, 0.7f, 0.6f, 1f),  // Verde pálido
            new Color(0.6f, 0.2f, 0.3f, 1f),  // Rojo ladrillo
            new Color(0.2f, 0.5f, 0.4f, 1f),  // Verde esmeralda
            new Color(0.4f, 0.6f, 0.9f, 1f),  // Azul cielo pálido
            new Color(0.8f, 0.4f, 0.6f, 1f),  // Rosa antiguo
            new Color(0.6f, 0.7f, 0.3f, 1f),  // Verde manzana
            new Color(0.9f, 0.2f, 0.4f, 1f),  // Fucsia fuerte
            new Color(0.7f, 0.5f, 0.3f, 1f),  // Marrón caramelo
            new Color(0.3f, 0.6f, 0.7f, 1f),  // Azul suave
            new Color(0.2f, 0.3f, 0.5f, 1f),  // Azul oscuro frío
            new Color(0.9f, 0.9f, 0.2f, 1f),  // Amarillo brillante
            new Color(0.5f, 0.5f, 0.8f, 1f),  // Lila suave
            new Color(0.6f, 0.4f, 0.7f, 1f),  // Morado lavanda
            new Color(0.2f, 0.8f, 0.5f, 1f)   // Verde mar
        };    
    public Color generatedColor; // Cadena p�blica para almacenar el n�mero aleatorio generado.
    private ISessionClientsProvider sessionClientsProvider;

    public void Inject(ISessionClientsProvider sessionClientsProvider)
    {
        this.sessionClientsProvider = sessionClientsProvider;
    }


    public override void Execute(string data)
    {
        // Generamos el n�mero solo si no se recibe uno de la red.
        string randomData = string.IsNullOrEmpty(data) ? GenerateRandom() : data;
        
        generatedColor = customColors.ElementAt(int.Parse(randomData));
        
        Debug.Log(randomData);
        if (codeDisplayDebug != null) // Verifica si el objeto TextMeshPro est� asignado.
        {
            codeDisplayDebug.text = generatedColor.ToString();
        }
        
        base.Execute(randomData); // Env�a el dato aleatorio o sincronizado a la clase base para su difusi�n.
       
        
        
        //LocalExecuteImplementation(randomData);
        

    }

    protected override void LocalExecuteImplementation(string data)
    {


    }
    
    
    private IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        
    }

    private string GenerateRandom()
    {
        return Random.Range(0, 49).ToString(); // Genera un n�mero aleatorio entre 1 y 50.
    }
}
