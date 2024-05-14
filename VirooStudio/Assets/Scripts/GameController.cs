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

        public GameObject lineSpawner;
        [SerializeField] private int subdivisions = 10; 
        
        private string idPrefix = "uniqueId_{0}";
        private int currentIndex = 0;
        

        public float curveHeightFactor; // Factor que controla la altura del arco
        public Vector3 curveDirection; // Dirección de la curvatura
        public float curveDistance; // Distancia de desplazamiento en la dirección de la curvatura

        public Dictionary<string, GameObject[]> players = new Dictionary<string, GameObject[]>();
        
        
        //Canvas para logs en Singleplayer
        
       
        [SerializeField] int maxLines = 8;
        [SerializeField] TextMeshProUGUI debugLogText;
        private Queue<string> queue = new Queue<string>();

        void OnEnable()
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
        }
        
        
        //TODO: Cables desaparecen en desconexion
        //TODO: conectar cable de entrada A a mando cuando haya una entrada seleccionada
        
        
        private void Start()
        {
            //3 manometros, 1 valvula manual, 1 valvula presion, 1 valvula reguladora canal
            
            solution.Add(new string[] {"Manometro4IN - Entrada_B", "ValvulaManual4IN - Entrada_B"});
            
            solution.Add(new string[] {"ValvulaManual4IN - Entrada_T", "Manometro4IN - Entrada_B"});
            solution.Add(new string[] {"ValvulaManual4IN - Entrada_P", "ValvulaReguladoraCanal2IN - Entrada_B"});
            
            solution.Add(new string[] {"ValvulaReguladoraCanal2IN - Entrada_A", "Manometro4IN - Entrada_B"});
            solution.Add(new string[] {"Manometro4IN - Entrada_T", "ValvulaPresion2IN - Entrada_P"});
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
                
                List<GameObject> inputs = CheckAction.FindChildrenWithTag(component.transform.Find("Entradas").gameObject, "Finish");
                foreach (GameObject input in inputs)
                {
                    input.GetComponent<InOutController>().linkedWith = "";
                    input.GetComponent<InOutController>().linkedWithIn = "";
                    input.GetComponent<InOutController>().isSelected = false;
                }
                
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
            UnselectAll();
        }

        
        public void DrawLine(Transform pointA, Transform pointB)
        {
            GameObject line = new GameObject();
            line.name = "Cable " + pointA.name + " - " + pointB.name;
            line.AddComponent<LineRenderer>();
            line.GetComponent<LineRenderer>().positionCount = subdivisions + 2;
            
            
            Vector3[] curvePoints = new Vector3[subdivisions + 2]; // Incrementado en 1 para incluir pointA y pointB

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
                curvePoints[i] += curveDirection * maxCurveHeight * tHeight;
            }

            line.GetComponent<LineRenderer>().SetPositions(curvePoints);
            line.GetComponent<LineRenderer>().startWidth = 0.01f;
            line.GetComponent<LineRenderer>().endWidth= 0.01f;
            
            line.GetComponent<LineRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            line.GetComponent<LineRenderer>().SetColors(c1, c2);
            
            
            
            Debug.Log(pointA.transform.position.x + " / " + pointB.transform.position.x);
        }

        public string GetUniqueId()
        {
            return string.Format(idPrefix, currentIndex++);
        }
    }
    
}
