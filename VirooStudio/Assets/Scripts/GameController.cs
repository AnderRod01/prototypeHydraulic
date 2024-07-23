using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefaultNamespace;
using Serilog;
using TMPro;
using Unity.VisualScripting.Community.Libraries.Humility;
using UnityEditor;
using UnityEngine;
using Viroo.UI;
using VirooLab.Actions;
using Virtualware.Networking.Client.SessionManagement;
using Random = UnityEngine.Random;


namespace gameControllerNamespace
{
    public class GameController : MonoBehaviour
    {
        public string selectedComponent = "";
        public string selectedInput = "";
    
        public GameObject[] availableComponents;
    
        public List<string[]> solution = new List<string[]>();
    
        [SerializeField] private GameObject canvasCorrect;
        [SerializeField] private GameObject canvasIncorrect;

        [SerializeField] private Color c1, c2;
        [SerializeField] private int subdivisions = 10; 
        
        private string idPrefix = "uniqueId_{0}";
        private int currentIndex = 0;
        
        public int curveResolution = 30;
        public float curveHeight = 0.3f;
        
        //public float curveHeightFactor; // Factor que controla la altura del arco
        //public Vector3 curveDirection; // Dirección de la curvatura
        //public float curveDistance; // Distancia de desplazamiento en la dirección de la curvatura

        public Dictionary<string, string[]> players = new Dictionary<string, string[]>();
        
        
        //Canvas para logs en Singleplayer
        
       
        //[SerializeField] int maxLines = 8;
        //[SerializeField] TextMeshProUGUI debugLogText;
        //private Queue<string> queue = new Queue<string>();

        /*void OnEnable()
        {
            Application.logMessageReceivedThreaded += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            // Delete oldest message
            if (queue.Count >= maxLines) queue.Dequeue();

            queue.Enqueue(logString);

            var builder = new StringBuilder();
            foreach (string st in queue)
            {
                builder.Append(st).Append("\n");
            }

            debugLogText.text = builder.ToString();
        }*/
        
        public string layerName = "Water";

        GameObject[] GetObjectsInLayer(int layer)
        {
            // Obtener todos los GameObjects en la escena
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
        
            // Crear una lista para almacenar los objetos en la capa específica
            List<GameObject> objectsInLayer = new List<GameObject>();

            // Filtrar los objetos por la capa especificada
            foreach (GameObject obj in allObjects)
            {
                if (obj.layer == layer)
                {
                    objectsInLayer.Add(obj);
                }
            }

            // Convertir la lista a un array y devolverla
            return objectsInLayer.ToArray();
        }
        
        private void Start()
        {
           /* 
            // Obtener el índice de la capa a partir del nombre de la capa
            int layer = LayerMask.NameToLayer(layerName);

            // Llamar al método para encontrar todos los objetos en esa capa
            GameObject[] objectsInLayer = GetObjectsInLayer(layer);

            // Imprimir el nombre de cada objeto encontrado
            foreach (GameObject obj in objectsInLayer)
            {
                Debug.Log("OBJETO EN CAPA WATER: " + obj.name);
            }*/
           
           
           
           
           
           //3 manometros, 1 valvula manual, 1 valvula presion, 1 valvula reguladora canal
            
            solution.Add(new string[] {"PistonLateral2IN - Entrada_A", "Manometro4IN - Entrada_T"});
            
            //MANOMETRO NEGRO
            solution.Add(new string[] {"PistonLateral2IN - Entrada_B", "Manometro4IN - Entrada_A"});
            solution.Add(new string[] {"Manometro4IN - Entrada_B", "Manometro4IN - Entrada_T1"});
            solution.Add(new string[] {"Manometro4IN - Entrada_P", "ValvulaReductoraPresion3IN - Entrada_P"});
            
            
            
            solution.Add(new string[] {"ValvulaReductoraPresion3IN - Entrada_T", "Tanque2IN - Entrada_B"});
            solution.Add(new string[] {"ValvulaReductoraPresion3IN - Entrada_A", "Manometro4IN - Entrada_A"});
            
            solution.Add(new string[] {"Tanque2IN - Entrada_A", "ValvulaManual4IN - Entrada_T"});
            
            solution.Add(new string[] {"Manometro4IN - Entrada_B", "ValvulaManual4IN - Entrada_B"});
            solution.Add(new string[] {"ValvulaManual4IN - Entrada_A", "Manometro4IN - Entrada_P"});
            solution.Add(new string[] {"ValvulaManual4IN - Entrada_P", "ValvulaReguladoraCanal2IN - Entrada_B"});

            solution.Add(new string[] {"ValvulaReguladoraCanal2IN - Entrada_A", "Manometro4IN - Entrada_A"});
            
            solution.Add(new string[] {"Manometro4IN - Entrada_P", "PitorroMesa2IN - Entrada_P"});
            solution.Add(new string[] {"Manometro4IN - Entrada_B", "ValvulaLimitadoraPresion2IN - Entrada_P"});
            solution.Add(new string[] {"PitorroMesa2IN - Entrada_P", "ValvulaLimitadoraPresion2IN - Entrada_T"});
            

        }
    
        public void UnselectAll()
        {
            availableComponents = GameObject.FindGameObjectsWithTag("Respawn");
            
            selectedComponent = "";
            selectedInput = "";
            
            foreach (GameObject component in availableComponents)
            {
                //component.GetComponent<Renderer>().material.color = component.GetComponent<ComponentController>().originalColor;
                component.GetComponent<ComponentController>().linkedWith = "";
                
                List<GameObject> inputs = CheckAction.FindChildrenWithTag(component.transform.GetChild(0).transform.Find("Entradas").gameObject, "Finish");
                
                foreach (GameObject input in inputs)
                {
                    input.GetComponent<InOutController>().linkedWith = "";
                    input.GetComponent<InOutController>().linkedWithIn = "";
                    input.GetComponent<InOutController>().isSelected = false;
                    input.GetComponent<InOutController>().selectionPlayer = "";
                }
                component.GetComponent<ComponentController>().enableCollider();
            }
            
            
            GameObject[] cables = (GameObject[])FindObjectsOfType(typeof(GameObject));
            foreach (GameObject cable in cables)
            {
                if (cable.name.Contains("Cable"))
                {
                    Destroy(cable);
                }
            }
        }
    
        public IEnumerator ShowCorrect()
        {
            Debug.Log("CORRECT");
            canvasCorrect.GetComponent<Canvas>().enabled = true;
            canvasCorrect.GetComponentInChildren<TMP_Text>().text = "CORRECTO!";
            yield return new WaitForSeconds(3f);
            canvasCorrect.GetComponent<Canvas>().enabled = false;
            UnselectAll();
        }
        
        public IEnumerator ShowIncorrect()
        {
            Debug.Log("INCORRECT");
            canvasIncorrect.GetComponent<Canvas>().enabled = true;
            canvasIncorrect.GetComponentInChildren<TMP_Text>().text = "INCORRECTO!";
            yield return new WaitForSeconds(3f);
            canvasIncorrect.GetComponent<Canvas>().enabled = false;
            //UnselectAll();
        }

        
        public void DrawLine(Transform objectA, Transform objectB)
        {
            GameObject line = new GameObject();
            
            line.name = "Cable " + objectA.GetComponent<InOutController>().component.componentName + " - " + objectA.name
                + " / " + objectB.GetComponent<InOutController>().component.componentName + " - " + objectB.name;
            
            line.AddComponent<LineRenderer>();
            line.GetComponent<LineRenderer>().positionCount = curveResolution;
            
            
            /*Vector3[] curvePoints = new Vector3[subdivisions + 2]; // Incrementado en 1 para incluir pointA y pointB

            // Primer punto en pointA
            curvePoints[0] = pointA.transform.position;

            // Segundo punto a una distancia curveDistance en la dirección de curveDirection
            curvePoints[1] = pointA.transform.position + curveDirection * curveDistance;

            // Último punto en pointB
            curvePoints[subdivisions + 1] = pointB.transform.position;

            // Penúltimo punto a una distancia curveDistance en la dirección opuesta de curveDirection
            curvePoints[subdivisions] = pointB.transform.position + curveDirection * curveDistance;

            // Calcular la altura máxima para los puntos intermedios
            float maxCurveHeight = Mathf.Max(pointA.transform.position.y, pointB.transform.position.y) + curveHeightFactor;

            // Calcular los puntos intermedios
            for (int i = 2; i < subdivisions; i++)
            {
                float t = (float)i / subdivisions;
                curvePoints[i] = Vector3.Lerp(curvePoints[1], curvePoints[subdivisions], t);

                // Ajustar la altura para asegurar que estén por encima de los puntos inicial y final
                float tHeight = Mathf.Sin(t * Mathf.PI);
                curvePoints[i] += curveDirection * maxCurveHeight * tHeight/2;
            }

            line.GetComponent<LineRenderer>().SetPositions(curvePoints);
            line.GetComponent<LineRenderer>().startWidth = 0.01f;
            line.GetComponent<LineRenderer>().endWidth= 0.01f;
            
            line.GetComponent<LineRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            line.GetComponent<LineRenderer>().SetColors(c1, c2);
            
            
            
            Debug.Log(pointA.transform.position.x + " / " + pointB.transform.position.x);*/
            line.GetComponent<LineRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            //line.GetComponent<LineRenderer>().SetColors(c1,c2);
            SetLineColors(line.GetComponent<LineRenderer>());
            
            Vector3 startPoint = objectA.position;
            Vector3 endPoint = objectB.position;

            Vector3 startDirection = objectA.TransformDirection(Vector3.up);
            Vector3 endDirection = objectB.TransformDirection(Vector3.up);

            Vector3 controlPointA = startPoint + startDirection * curveHeight;
            Vector3 controlPointB = endPoint + endDirection * curveHeight;

            for (int i = 0; i < curveResolution; i++)
            {
                float t = i / (float)(curveResolution - 1);
                Vector3 point = CalculateBezierPoint(t, startPoint, controlPointA, controlPointB, endPoint);
                line.GetComponent<LineRenderer>().SetPosition(i, point);
                line.GetComponent<LineRenderer>().startWidth = 0.01f;
                line.GetComponent<LineRenderer>().endWidth= 0.01f;
            }
            
            
        }
        
        Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0; // (1-t)^3 * p0
            p += 3 * uu * t * p1; // 3 * (1-t)^2 * t * p1
            p += 3 * u * tt * p2; // 3 * (1-t) * t^2 * p2
            p += ttt * p3; // t^3 * p3

            return p;
        }
        void SetLineColors(LineRenderer lineRenderer)
        {
            Gradient gradient = new Gradient();

            GradientColorKey[] colorKey = new GradientColorKey[4];
            GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];

            // First 10% is black
            colorKey[0].color = c1;
            colorKey[0].time = 0.05f;

            Color randomColor = new Color(Random.value, Random.value, Random.value);
            colorKey[1].color = randomColor;
            colorKey[1].time = 0.1f;
            colorKey[2].color = randomColor;
            colorKey[2].time = 0.89f;

            colorKey[3].color = c1;
            colorKey[3].time = 0.95f;

            // Alpha is always 1
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            lineRenderer.colorGradient = gradient;
        }

        public string GetUniqueId()
        {
            return string.Format(idPrefix, currentIndex++);
        }
    }
    
}
