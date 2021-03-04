using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Public Variables")]

    public List<GameObject> activeTargetsList = new List<GameObject>();

    public List<Transform> activeGatesList = new List<Transform>();

    public void StartTargetsMovement()
    {
        Debug.Log("Active targets : " + activeTargetsList.Count);

        Debug.Log("Active gates : " + activeGatesList.Count);

        for (int x = 0; x < activeTargetsList.Count; x++)
        {
            activeTargetsList[x].GetComponent<NavMeshAgent>().isStopped = false;

            activeTargetsList[x].GetComponent<NavMeshAgent>().SetDestination(activeGatesList[x].position);

        }

    }

}
