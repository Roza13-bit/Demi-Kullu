using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupSingleton : MonoBehaviour
{
    [SerializeField] private HeroScriptableObject heroPrefab;

    [SerializeField] private TargetScriptableObject targetPrefab;

    [SerializeField] private GameObject mazePrefab;

    private List<Transform> targetSpawnTransformList = new List<Transform>();

    private GameObject _heroPrefab;

    private GameObject _targetPrefab;

    private GameObject _mazePrefab;

    public void InstantiateCurrentLevelObjects()
    {
        _heroPrefab = Instantiate(heroPrefab.heroPrefab);

        _mazePrefab = Instantiate(mazePrefab);

        foreach (Transform child in _mazePrefab.transform)
        {
            if (child.CompareTag("Spawn"))
            {
                targetSpawnTransformList.Add(child);

                Debug.Log("Number of transform points found : " + targetSpawnTransformList.Count);

            }
            else
            {

            }

        }

    }

    public void PositionGameObjects()
    {
        

    }

    public void SetLevelUIGraphics()
    {


    }

}
