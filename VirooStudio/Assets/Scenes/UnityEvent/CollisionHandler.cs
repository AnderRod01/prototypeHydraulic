using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public List<GameObject> piezas1;
    public List<GameObject> piezas2;

    void Start()
    {
        FindPieces();
    }

    // Función para encontrar todos los GameObjects con VirooTag "Pieza 1" y "Pieza 2"
    public void FindPieces()
    {

        Debug.Log("FindPieces");
        piezas1 = new List<GameObject>();
        piezas2 = new List<GameObject>();

        VirooTag[] allObjects = FindObjectsOfType<VirooTag>();
        foreach (VirooTag obj in allObjects)
        {
            if (obj.Id == "Pieza 1")
            {
                piezas1.Add(obj.gameObject);
                Debug.Log("Pieza 1 encontrada: " + obj.gameObject.name);
            }
            else if (obj.Id == "Pieza 2")
            {
                piezas2.Add(obj.gameObject);
                Debug.Log("Pieza 2 encontrada: " + obj.gameObject.name);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión detectada: " + collision.gameObject.name);

        foreach (GameObject pieza1 in piezas1)
        {
            // Verifica si el collider de Pieza 1 es el que está colisionando
            Collider sphereColliderPieza1 = pieza1.transform.Find("Collider/Sphere Collider").GetComponent<Collider>();
            if (collision.collider == sphereColliderPieza1)
            {
                Debug.Log("Colisión con Pieza 1: " + pieza1.name);

                foreach (ContactPoint contact in collision.contacts)
                {
                    Debug.Log("Punto de contacto: " + contact.otherCollider.name);

                    // Verifica si el collider de Pieza 2 es el que está colisionando
                    foreach (GameObject pieza2 in piezas2)
                    {
                        Collider sphereColliderPieza2 = pieza2.transform.Find("Sphere/Sphere Collider").GetComponent<Collider>();
                        if (contact.otherCollider == sphereColliderPieza2)
                        {
                            Debug.Log("Colisión con Pieza 2: " + pieza2.name);

                            // Activa el objeto "Sphere" dentro de "Pieza 1"
                            Transform spherePieza1 = pieza1.transform.Find("Sphere");
                            if (spherePieza1 != null)
                            {
                                spherePieza1.gameObject.SetActive(true);
                                Debug.Log("Activando Sphere en Pieza 1: " + spherePieza1.name);
                            }

                            // Destruye Pieza 2 un segundo después
                            Destroy(pieza2, 1.0f);
                            Debug.Log("Pieza 2 será destruida: " + pieza2.name);
                        }
                    }
                }
            }
        }
    }
}
