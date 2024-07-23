using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = UnityEngine.Windows.Input;
using Viroo.Interactions;
using Virtualware.Networking.Client.SessionManagement;
using System.Linq;


public class ValvulaEstranguladoraCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameObject valvulaEstranguladora;
    public void HandleCollision()
    {
        Destroy(valvulaEstranguladora, 0.2f);
    }

}
