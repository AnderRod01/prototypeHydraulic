using System.Collections;
using UnityEngine;
using Viroo.Interactions;
using Viroo.Networking;

public class GetInteractorPlayer : MonoBehaviour
{
    [SerializeField]
    private BroadcastObjectAction action;

    private IPlayerProvider playerProvider;

    protected void Inject(IPlayerProvider playerProvider)
    {
        this.playerProvider = playerProvider;
    }

    private void Awake()
    {
        Wait(1f);
        this.QueueForInject();
    }

    public void ExecuteActionProxy()
    {
        IPlayer localPlayer = playerProvider.GetLocalPlayer();

        //Debug.Log("Player executing action: " +  localPlayer.ClientId);
        
        //Como el método se llama a través de un UnityEventNonBroadcastAction  este método se llamaría solo en el cliente que hace click por lo que ejecutaríamos la acción en broadcast llamando al Execute()
        action.Execute(localPlayer.ClientId);
    }
    
    private IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
    }
}