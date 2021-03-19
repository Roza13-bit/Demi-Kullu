using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UIClass;

public class GameSetupSingleton : MonoBehaviour
{
    [Header("Cached References")]

    [SerializeField] private HeroScriptableObject heroPrefabSO;

    [SerializeField] private TargetScriptableObject targetPrefabSO;

    [SerializeField] private GameObject mazePrefab;

    [SerializeField] private GameManager gameManagerSC;

    [SerializeField] private StateManager stateManagerSC;

    [SerializeField] private UIManager uiManagerSC;

    // Private Variables.

    private List<Transform> targetSpawnTransformList = new List<Transform>();

    private GameObject _heroPrefab;

    private GameObject _targetPrefab;

    private GameObject _mazePrefab;

    // Instantiate the current level's objects.
    // Add the transform points into a list.
    // Add the targets into a list in the game manager class.
    public void InstantiateCurrentLevelObjects()
    {
        uiManagerSC.lightAttackSO = heroPrefabSO.lightAttackSO;

        uiManagerSC.heavyAttackSO = heroPrefabSO.heavyAttackSO;

        uiManagerSC.ultimateAttackSO = heroPrefabSO.ultimateAttackSO;

        _heroPrefab = Instantiate(heroPrefabSO.heroPrefab);

        gameManagerSC.heroPrefab = _heroPrefab;

        _mazePrefab = Instantiate(mazePrefab);

        foreach (Transform child in _mazePrefab.transform)
        {
            if (child.CompareTag("Spawn"))
            {
                targetSpawnTransformList.Add(child);

                Debug.Log("Number of transform points found : " + targetSpawnTransformList.Count);

            }
            else if (child.CompareTag("EndGate"))
            {
                gameManagerSC.activeGatesList.Add(child);

            }

        }

        for (int x = 0; x < targetSpawnTransformList.Count; x++)
        {
            _targetPrefab = Instantiate(targetPrefabSO.targetPrefab);

            gameManagerSC.activeTargetsList.Add(_targetPrefab);

            Debug.Log("Number of active targets found : " + gameManagerSC.activeTargetsList.Count);

        }

        Debug.Log("Finished instantiating objects.");

    }

    // Position the targets to the maze spawn transform points.
    // Rotate the targets to the spawn point rotation.
    public void PositionGameObjects()
    {
        for (int x = 0; x < targetSpawnTransformList.Count; x++)
        {
            gameManagerSC.activeTargetsList[x].transform.position = targetSpawnTransformList[x].transform.position;

            gameManagerSC.activeTargetsList[x].transform.rotation = targetSpawnTransformList[x].transform.rotation;

        }

    }

    public void SwitchStateFromGameToPlay()
    {
        // stateManagerSC.PlayStartedEvent.Invoke();

        Destroy(gameObject);

    }

}
