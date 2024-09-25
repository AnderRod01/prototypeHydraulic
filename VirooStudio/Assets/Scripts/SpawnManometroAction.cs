using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;

public class SpawnManometroAction : BroadcastObjectAction
{
    private ISessionClientsProvider sessionClientsProvider;

    public void Inject(ISessionClientsProvider sessionClientsProvider)
    {
        this.sessionClientsProvider = sessionClientsProvider;
    }


    public override void Execute(string data)
    {
        this.gameObject.SetActive(true);


    }

    protected override void LocalExecuteImplementation(string data)
    {
        //throw new System.NotImplementedException();
    }
}

