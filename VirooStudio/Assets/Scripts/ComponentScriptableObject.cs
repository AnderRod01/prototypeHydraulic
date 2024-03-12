using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class ComponentScriptableObject : ScriptableObject
{
    public string componentName;

    public int numberOfInputs;
    public int numberOfOutputs;
}
