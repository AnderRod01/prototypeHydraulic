using UnityEngine;
using TMPro;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;

public class RandomNumGeneratorBroadcast_Base : BroadcastObjectAction
{
    [SerializeField] private TextMeshPro codeDisplayDebug; // Texto3D para mostrar el n�mero aleatorio en escena con fines de depuraci�n.
    public string generatedCode; // Cadena p�blica para almacenar el n�mero aleatorio generado.
    private ISessionClientsProvider sessionClientsProvider;

    public void Inject(ISessionClientsProvider sessionClientsProvider)
    {
        this.sessionClientsProvider = sessionClientsProvider;
    }

    private void Start()
    {

    }

    public override void Execute(string data)
    {
        // Generamos el n�mero solo si no se recibe uno de la red.
        string randomData = string.IsNullOrEmpty(data) ? GenerateRandom() : data;
        base.Execute(randomData); // Env�a el dato aleatorio o sincronizado a la clase base para su difusi�n.
        UpdateLocalData(randomData);
    }

    protected override void LocalExecuteImplementation(string data)
    {
        // Aqu� solo actualizamos el n�mero recibido desde la red.
        UpdateLocalData(data);
    }

    private void UpdateLocalData(string data)
    {
        generatedCode = data;
        if (codeDisplayDebug != null) // Verifica si el objeto TextMeshPro est� asignado.
        {
            codeDisplayDebug.text = generatedCode;
        }
    }

    private string GenerateRandom()
    {
        return Random.Range(1, 50).ToString(); // Genera un n�mero aleatorio entre 1 y 50.
    }
}
