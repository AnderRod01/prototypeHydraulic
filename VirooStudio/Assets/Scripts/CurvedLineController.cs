using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurvedLineController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public int subdivisions = 10;
    public float curveHeightFactor = 1f; // Factor que controla la altura del arco
    public Vector3 curveDirection = Vector3.up; // Dirección de la curvatura
    public float curveDistance = 1f; // Distancia de desplazamiento en la dirección de la curvatura
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = subdivisions + 2; // Incrementado en 1 para incluir pointA y pointB
        //DrawCurve();
    }

    public void DrawCurve(Transform pointA, Transform pointB)
    {
        Vector3[] curvePoints = new Vector3[subdivisions + 2]; // Incrementado en 1 para incluir pointA y pointB

        // Primer punto en pointA
        curvePoints[0] = pointA.position;

        // Segundo punto a una distancia curveDistance en la dirección de curveDirection
        curvePoints[1] = pointA.position + curveDirection * curveDistance;

        // Último punto en pointB
        curvePoints[subdivisions + 1] = pointB.position;

        // Penúltimo punto a una distancia curveDistance en la dirección opuesta de curveDirection
        curvePoints[subdivisions] = pointB.position + curveDirection * curveDistance;

        // Calcular la altura máxima para los puntos intermedios
        float maxCurveHeight = Mathf.Max(pointA.position.y, pointB.position.y) + curveHeightFactor;

        // Calcular los puntos intermedios
        for (int i = 2; i < subdivisions; i++)
        {
            float t = (float)i / subdivisions;
            curvePoints[i] = Vector3.Lerp(curvePoints[1], curvePoints[subdivisions], t);

            // Ajustar la altura para asegurar que estén por encima de los puntos inicial y final
            float tHeight = Mathf.Sin(t * Mathf.PI);
            curvePoints[i] += curveDirection * maxCurveHeight * tHeight;
        }

        lineRenderer.SetPositions(curvePoints);
    }
    
    Vector3 CalculateCurvePoint(float t)
    {
        // Use Lerp for linear interpolation between points A and B
        Vector3 lerpedPoint = Vector3.Lerp(pointA.position + curveDirection * curveDistance, pointB.position - curveDirection * curveDistance, t);

        // Calculate direction vector from pointA to pointB
        Vector3 direction = (pointB.position - pointA.position).normalized;

        // Calculate curve height based on the direction vector
        float curveHeight = curveHeightFactor * direction.magnitude;

        // Modify Y position to create a curve
        lerpedPoint += curveDirection * curveHeight * Mathf.Sin(t * Mathf.PI);

        return lerpedPoint;
    }

    void Update()
    {
        if (pointA != null && pointB != null)
        {
            //DrawCurve();
        }
    }
}
