using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaScript : MonoBehaviour
{
    [SerializeField] private Transform transformEntrada;

    private void Update()
    {
        transform.position = transformEntrada.position;
    }

    private void Start()
    {
        transformEntrada = GameObject.Find("Manometro4IN/Entradas/Entrada_A").transform;
    }
}
