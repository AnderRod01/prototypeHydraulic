using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public int numberOfPoints = 10;
    public bool fixStartPoint = false;
    public bool fixEndPoint = false;
    public float pointMass = 0.1f;
    public float pointDamping = 0.5f;
    public float pointStiffness = 1f;
    public float cableThickness = 0.1f;

    private GameObject[] cablePoints;
    private LineRenderer lineRenderer;

    void Start()
    {
        cablePoints = new GameObject[numberOfPoints + 2];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints + 2;

        // Crear puntos intermedios y conectarlos
        CreateCablePoints();
        ConnectCablePoints();
    }

    void Update()
    {
        // Actualizar posición de los puntos extremos
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(numberOfPoints + 1, endPoint.position);

        // Actualizar posiciones y renderizar el cable
        for (int i = 0; i < cablePoints.Length; i++)
        {
            lineRenderer.SetPosition(i, cablePoints[i].transform.position);
        }
    }

    void CreateCablePoints()
    {
        for (int i = 0; i < numberOfPoints + 2; i++)
        {
            GameObject cablePoint = new GameObject("CablePoint_" + i);
            Rigidbody rigidbody = cablePoint.AddComponent<Rigidbody>();
            rigidbody.mass = pointMass;
            rigidbody.drag = pointDamping;
            Vector3 localYStart = startPoint.up;
            Vector3 localYEnd = endPoint.up;
            Vector3 initialPosition = Vector3.Lerp(startPoint.position, endPoint.position, (float)i / (numberOfPoints + 1));
            cablePoint.transform.position = initialPosition;
            cablePoint.transform.parent = transform;

            cablePoints[i] = cablePoint;

            // Fijar extremos si es necesario
            if (i == 0 && fixStartPoint)
            {
                cablePoint.AddComponent<FixedJoint>().connectedBody = startPoint.GetComponent<Rigidbody>();
            }
            else if (i == numberOfPoints + 1 && fixEndPoint)
            {
                cablePoint.AddComponent<FixedJoint>().connectedBody = endPoint.GetComponent<Rigidbody>();
            }
        }
    }

    void ConnectCablePoints()
    {
        for (int i = 0; i < numberOfPoints + 1; i++)
        {
            SpringJoint springJoint = cablePoints[i].AddComponent<SpringJoint>();
            springJoint.connectedBody = cablePoints[i + 1].GetComponent<Rigidbody>();
            springJoint.spring = pointStiffness;
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.anchor = Vector3.zero;
            springJoint.connectedAnchor = Vector3.zero;
        }
    }
}
