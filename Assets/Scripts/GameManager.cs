using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Public Variables")]

    public List<GameObject> activeTargetsList = new List<GameObject>();

    public List<Transform> activeGatesList = new List<Transform>();

    public List<GameObject> touchedTargetsList = new List<GameObject>();

    public GameObject heroPrefab;

    public SkillScriptableObject lightAttackSO;

    public SkillScriptableObject heavyAttackSO;

    public SkillScriptableObject ultimateAttackSO;

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

    public void EnableHeroControllerScript()
    {
        heroPrefab.GetComponent<HeroController>().enabled = true;

    }

    public void TargetsListHeavyDamage()
    {
        foreach (GameObject target in touchedTargetsList)
        {
            target.GetComponent<TargetController>().UpdateSliderValue(heavyAttackSO.skillDamage);

            Debug.Log("Minus " + heavyAttackSO.skillDamage + " Damage");

        }

    }

    public void TargetsListUltimateDamage()
    {
        Debug.Log("Ultimate damage.");

        foreach (GameObject target in activeTargetsList)
        {
            target.GetComponent<TargetController>().UpdateSliderValue(ultimateAttackSO.skillDamage);

            Debug.Log("Minus " + ultimateAttackSO.skillDamage + " Damage");

        }

    }

}
