using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetController : MonoBehaviour
{
    private NavMeshAgent targetNavmesh;

    private void Start()
    {
        targetNavmesh = gameObject.GetComponent<NavMeshAgent>();

        targetNavmesh.isStopped = true;

    }

}
