using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;

public class EnableObjectBroadcast : BroadcastObjectAction
{
    
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
        
        //GetComponent(data)<ComponentSelector>.spawnButton.interactable = false);
        //spawnManager.spawnButton.GetComponent<BoxCollider>().enabled = false;
        
        

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
