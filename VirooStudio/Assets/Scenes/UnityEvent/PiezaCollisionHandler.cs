using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiezaCollisionHandler : MonoBehaviour
{
    private List<GameObject> piezas2;

    void Start()
    {
        FindPieces2();
    }

    // Función para encontrar todos los GameObjects con VirooTag "Pieza 2"
    void FindPieces2()
    {
        piezas2 = new List<GameObject>();

        VirooTag[] allObjects = FindObjectsOfType<VirooTag>();
        foreach (VirooTag obj in allObjects)
        {
            if (obj.Id == "Pieza 2")
            {
                piezas2.Add(obj.gameObject);
                Debug.Log("Pieza 2 encontrada: " + obj.gameObject.name);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("Colisión detectada: " + collision.gameObject.name);

        // Verifica si el collider de Pieza 1 es el que está colisionando
        Collider sphereColliderPieza1 = transform.Find("Collider").GetComponent<Collider>();
        if (collision.collider == sphereColliderPieza1)
        {
            Debug.Log("Colisión con Pieza 1: " + gameObject.name);

            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.Log("Punto de contacto: " + contact.otherCollider.name);

                // Verifica si el collider de Pieza 2 es el que está colisionando
                foreach (GameObject pieza2 in piezas2)
                {
                    Collider sphereColliderPieza2 = pieza2.transform.Find("Sphere").GetComponent<Collider>();
                    if (contact.otherCollider == sphereColliderPieza2)
                    {
                        Debug.Log("Colisión con Pieza 2: " + pieza2.name);

                        // Activa el objeto "Sphere" dentro de "Pieza 1"
                        Transform spherePieza1 = transform.Find("Sphere");
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
