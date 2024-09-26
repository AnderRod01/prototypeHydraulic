using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;

public class DeleteAction : MonoBehaviour
{
    
    public void Delete()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

}
