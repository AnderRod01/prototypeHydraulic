using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;

public class DeleteAction : BroadcastObjectAction
{
    

    private ISessionClientsProvider sessionClientsProvider;

    public void Inject(ISessionClientsProvider sessionClientsProvider)
    {
        this.sessionClientsProvider = sessionClientsProvider;
    }


    public override void Execute(string data)
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    protected override void LocalExecuteImplementation(string data)
    {
        //throw new System.NotImplementedException();
    }
}
