using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UIClass;

public class GameSetupSingleton : MonoBehaviour
{
    [Header("Cached References")]

    [SerializeField] private HeroScriptableObject heroPrefabSO;

    [SerializeField] private TargetScriptableObject targetGenericSO;

    [SerializeField] private TargetScriptableObject targetSkinnySO;

    [SerializeField] private TargetScriptableObject targetFatSO;

    [SerializeField] private LevelStatsScriptableObject levelStatsSO;

    [SerializeField] private GameObject mazePrefab;

    [SerializeField] private GameManager gameManagerSC;

    [SerializeField] private StateManager stateManagerSC;

    [SerializeField] private UIManager uiManagerSC;

    // Private Variables.

    private GameObject _heroPrefab;

    private GameObject _targetGenericPrefab;

    private GameObject _mazePrefab;

    // Instantiate the current level's objects.
    // Add the transform points into a list.
    // Add the targets into a list in the game manager class.
    public void InstantiateCurrentLevelObjects()
    {
        uiManagerSC.lightAttackSO = heroPrefabSO.lightAttackSO;

        uiManagerSC.heavyAttackSO = heroPrefabSO.heavyAttackSO;

        uiManagerSC.ultimateAttackSO = heroPrefabSO.ultimateAttackSO;

        gameManagerSC.lightAttackSO = heroPrefabSO.lightAttackSO;

        gameManagerSC.heavyAttackSO = heroPrefabSO.heavyAttackSO;

        gameManagerSC.ultimateAttackSO = heroPrefabSO.ultimateAttackSO;

        gameManagerSC.levelStatsSO = levelStatsSO;

        gameManagerSC.targetGenericSO = targetGenericSO;

        gameManagerSC.targetSkinnySO = targetSkinnySO;

        gameManagerSC.targetFatSO = targetFatSO;

        _heroPrefab = Instantiate(heroPrefabSO.heroPrefab);

        gameManagerSC.heroPrefab = _heroPrefab;

        _mazePrefab = Instantiate(mazePrefab);

        foreach (Transform child in _mazePrefab.transform)
        {
            if (child.CompareTag("Spawn"))
            {
                gameManagerSC.targetSpawnTransformList.Add(child);

                Debug.Log("Number of transform points found : " + gameManagerSC.targetSpawnTransformList.Count);

            }
            else if (child.CompareTag("EndGate"))
            {
                gameManagerSC.activeGatesList.Add(child);

            }

        }

        for (int x = 0; x < gameManagerSC.targetSpawnTransformList.Count; x++)
        {
            _targetGenericPrefab = Instantiate(targetGenericSO.targetPrefab);

            gameManagerSC.activeTargetsList.Add(_targetGenericPrefab);

            Debug.Log("Number of active targets found : " + gameManagerSC.activeTargetsList.Count);

        }

        Debug.Log("Finished instantiating objects.");

    }


    // Position the targets to the maze spawn transform points.
    // Rotate the targets to the spawn point rotation.
    public void PositionGameObjects()
    {
        for (int x = 0; x < gameManagerSC.targetSpawnTransformList.Count; x++)
        {
            gameManagerSC.activeTargetsList[x].transform.position = gameManagerSC.targetSpawnTransformList[x].transform.position;

            gameManagerSC.activeTargetsList[x].transform.rotation = gameManagerSC.targetSpawnTransformList[x].transform.rotation;

        }

    }


    // Destroy the setup singelton.
    public void SwitchStateFromGameToPlay()
    {
        // stateManagerSC.PlayStartedEvent.Invoke();

        Destroy(gameObject);

    }

}
