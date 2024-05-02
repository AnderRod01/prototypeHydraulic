using DefaultNamespace;
using gameControllerNamespace;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Viroo.Interactions;
using Viroo.UI;

namespace VirooLab.Actions
{
    public class LinkComponent : BroadcastObjectAction
    {

        [SerializeField] private InOutController input;
        
        protected override void OnInject()
        {

            string uniqueID = GameObject.Find("GameController").GetComponent<GameController>().GetUniqueId(); //Consultar un script o servicio que nos proporcione un id único y que sea el mismo en todas las instancias.

            System.Reflection.FieldInfo prop = typeof(ObjectAction).GetField("id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(this, uniqueID);

            base.OnInject();
        }

        protected override void LocalExecuteImplementation(string data)
        {
            //Debug.Log("Selecciono");
            input.MarkAsSelected(data);
        }
        
    }
}