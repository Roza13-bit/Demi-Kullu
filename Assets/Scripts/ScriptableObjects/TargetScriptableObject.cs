using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Target Settings")]

public class TargetScriptableObject : ScriptableObject
{
    public GameObject targetPrefab;

    public float targetSpeed;

    public float targetAccelaration;

    public float targetHealth;

}
