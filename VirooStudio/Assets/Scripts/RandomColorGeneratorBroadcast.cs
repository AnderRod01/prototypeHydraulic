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
    public Color currentColor; // Color sincronizado actual (para el cable a crear)
    private ISessionClientsProvider sessionClientsProvider;

    public void Inject(ISessionClientsProvider sessionClientsProvider)
    {
        this.sessionClientsProvider = sessionClientsProvider;
    }

    private void Start()
    {
        // Generar el color inicial para el primer cable
        GenerateAndSyncColor();
    }

    public override void Execute(string data)
    {
        // Si no se recibe un color de la red, generamos uno localmente
        string randomData = string.IsNullOrEmpty(data) ? GenerateRandom() : data;
        base.Execute(randomData);  // Difunde el color generado o recibido a todos los demás clientes.
        UpdateLocalColor(randomData);  // Actualiza el color localmente.
    }

    protected override void LocalExecuteImplementation(string data)
    {
        // Actualiza el color recibido desde la red
        UpdateLocalColor(data);
    }

    private void UpdateLocalColor(string data)
    {
        int colorIndex = int.Parse(data);  // Convierte el string a un índice
        currentColor = customColors[colorIndex];  // Selecciona el color de la lista

        // Opcional: muestra el color en pantalla para depuración
        if (codeDisplayDebug != null)
        {
            codeDisplayDebug.text = "Color actual: " + currentColor.ToString();
        }
    }

    private string GenerateRandom()
    {
        // Genera un índice aleatorio basado en la cantidad de colores disponibles
        return Random.Range(0, customColors.Count).ToString();
    }

    // Este método se llamará cuando crees un cable
    public void OnCableCreated()
    {
        // El cable utiliza el color "currentColor" que ya ha sido sincronizado
        // Aquí puedes aplicar el color "currentColor" al cable

        // Después de crear el cable, generamos el color para el siguiente cable
        GenerateAndSyncColor();
    }

    private void GenerateAndSyncColor()
    {
        string randomData = GenerateRandom(); // Genera un nuevo índice para el siguiente color
        Execute(randomData); // Sincroniza el nuevo color
    }
}
