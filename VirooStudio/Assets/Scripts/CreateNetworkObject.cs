using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Virtualware.Networking.Client;
using Virtualware.Networking.Client.SceneManagement;
using Networking.Messages;

public class CreateNetworkObject : MonoBehaviour
{
    [SerializeField]
            private string instantiatePrefabId = string.Empty;
    
            [SerializeField]
            private Transform instantiatePosition = default;
    
            private NetworkObjectsService networkObjectsService;
            private INetworkScenesService networkScenesService;
    
            public Transform InstantiatePosition { get => instantiatePosition; set => instantiatePosition = value; }
    
            public string InstantiatePrefabId { get => instantiatePrefabId; set => instantiatePrefabId = value; }
    
            protected void Inject(NetworkObjectsService networkObjectsService, INetworkScenesService networkScenesService)
            {
                this.networkObjectsService = networkObjectsService;
                this.networkScenesService = networkScenesService;
            }
    
            protected void Awake()
            {
                this.QueueForInject();
            }
    
            public async void Create(ComponentSelector spawnManager)
            {
                //spawnManager.spawnButton.interactable = (spawnManager.currentIndex == 4 && spawnManager.currentIndexestranguladora == spawnManager.estranguladoras.Length) || 
                                                        //(spawnManager.currentIndex == 5 && spawnManager.currentIndexManometro ==  spawnManager.manometros.Length);


                
                if (spawnManager.currentIndex == 4)
                {
                    spawnManager.estranguladoras[spawnManager.currentIndexestranguladora].GetComponent<SpawnEstranguladoraAction>().Execute(string.Empty);
                    spawnManager.currentIndexestranguladora++;
                    
                    if ((spawnManager.currentIndex == 4 && spawnManager.currentIndexestranguladora == spawnManager.estranguladoras.Length - 1) ||
                        (spawnManager.currentIndex == 5 && spawnManager.currentIndexManometro ==  spawnManager.manometros.Length))
                    {
                        spawnManager.spawnButton.interactable = false;
                        spawnManager.spawnButton.GetComponent<BoxCollider>().enabled = false;
                    }
                    else
                    {
                        spawnManager.spawnButton.interactable = true;
                        spawnManager.spawnButton.GetComponent<BoxCollider>().enabled = true;
                    }
                    return;
                }
                
                if (spawnManager.currentIndex == 5)
                {
                    spawnManager.manometros[spawnManager.currentIndexManometro].GetComponent<SpawnManometroAction>().Execute(string.Empty);
                    spawnManager.currentIndexManometro++;
                    
                    if ((spawnManager.currentIndex == 4 && spawnManager.currentIndexestranguladora == spawnManager.estranguladoras.Length - 1) ||
                        (spawnManager.currentIndex == 5 && spawnManager.currentIndexManometro ==  spawnManager.manometros.Length))
                    {
                        spawnManager.spawnButton.interactable = false;
                        spawnManager.spawnButton.GetComponent<BoxCollider>().enabled = false;
                    }
                    else
                    {
                        spawnManager.spawnButton.interactable = true;
                        spawnManager.spawnButton.GetComponent<BoxCollider>().enabled = true;
                    }
                    return;
                }
                
                CreateSessionObjectResponse response = await networkObjectsService.CreateDynamicObject(
                    spawnManager.currentIndex + "",
                       InstantiatePosition.position,
                       InstantiatePosition.rotation,
                       requestAuthority: true,
                       isPersistent: true,
                       networkScenesService.CurrentActiveScene.SceneName);
    
                if (!response.Success)
                {
                    Debug.LogError($"Error creating Object {InstantiatePrefabId}");
                }
            }
}
