using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIClass;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Public Variables")]

    [SerializeField] private StateManager stateManagerSC;

    // Hero prefab.
    public GameObject heroPrefab;

    // Runtime lists.
    public List<GameObject> newTargetsList = new List<GameObject>();

    public List<GameObject> activeTargetsList = new List<GameObject>();

    public List<Transform> activeGatesList = new List<Transform>();

    public List<GameObject> touchedTargetsList = new List<GameObject>();

    public List<Transform> targetSpawnTransformList = new List<Transform>();

    // Scriptable objects.
    public SkillScriptableObject lightAttackSO;

    public SkillScriptableObject heavyAttackSO;

    public SkillScriptableObject ultimateAttackSO;

    public LevelStatsScriptableObject levelStatsSO;

    public TargetScriptableObject targetGenericSO;

    public TargetScriptableObject targetSkinnySO;

    public TargetScriptableObject targetFatSO;

    // Targets local variables.
    private GameObject _targetGenericPrefab;

    private GameObject _targetSkinnyPrefab;

    private GameObject _targetFatPrefab;

    private bool isSpawningTargets;


    // Enable the hero controller when the play starts.
    public void EnableHeroControllerScript()
    {
        heroPrefab.GetComponent<HeroController>().enabled = true;

        stateManagerSC.StartSpawningStartedState();

    }


            // ~~ Deal damage to targets ~~

    // Deal heavy damage to touched enemies.
    public void TargetsListHeavyDamage()
    {
        for (int x = 0; x < touchedTargetsList.Count; x++)
        {
            if (touchedTargetsList[x] != null)
            {
                touchedTargetsList[x].GetComponent<TargetController>().UpdateSliderValue(heavyAttackSO.skillDamage);

            }

            Debug.Log("Minus " + heavyAttackSO.skillDamage + " Damage");

        }

    }


    // Deal ultimate damage to all enemies.
    public void TargetsListUltimateDamage()
    {
        for (int x = 0; x < activeTargetsList.Count; x++)
        {
            activeTargetsList[x].GetComponent<TargetController>().PlayUltimateAttackVFX();

            activeTargetsList[x].GetComponent<TargetController>().UpdateSliderValue(ultimateAttackSO.skillDamage);

            Debug.Log("Minus " + ultimateAttackSO.skillDamage + " Damage");

        }

    }


            // ~~ Targets spawning & movement logic ~~

    // Initialize movement for the starting enemies.
    public void StartTargetsMovement()
    {
        for (int x = 0; x < activeTargetsList.Count; x++)
        {
            if (!activeTargetsList[x].GetComponent<NavMeshAgent>().hasPath)
            {
                activeTargetsList[x].GetComponent<NavMeshAgent>().isStopped = false;

                activeTargetsList[x].GetComponent<NavMeshAgent>().SetDestination(activeGatesList[x].position);

            }
            else
            {
                return;

            }

        }

    }


    // Initialize the game loop timer.
    private IEnumerator StartGameLoopTimer()
    {
        int levelSecondsLength = levelStatsSO.levelSecondsTime;

        isSpawningTargets = true;

        Debug.Log("    ~~ Game Timer Started ~~    ");

        yield return new WaitForSeconds(levelSecondsLength);

        Debug.Log("    ~~ Game Timer Ended ~~    ");

        isSpawningTargets = false;

        while (true)
        {
            Debug.Log("Still has active targets.");

            if (activeTargetsList.Count == 0)
            {
                UIManager.ActivateNextStagePanel();

                yield break;

            }
            else
            {
                yield return new WaitForSeconds(1f);

            }

        }

    }

    public void StartGameLoopTimerEvent()
    { StartCoroutine(StartGameLoopTimer()); }


    // Spawn generic targets.
    public IEnumerator StartSpawningGenericTargets()
    {
        float spawnRate = levelStatsSO.genericTargetSpawnRate;

        while (true && isSpawningTargets)
        {
            yield return new WaitForSeconds(spawnRate);

            if (isSpawningTargets)
            {
                for (int x = 0; x < targetSpawnTransformList.Count; x++)
                {
                    _targetGenericPrefab = Instantiate(targetGenericSO.targetPrefab, targetSpawnTransformList[x].position, targetSpawnTransformList[x].rotation, null);

                    activeTargetsList.Add(_targetGenericPrefab);

                    yield return new WaitForSeconds(0.2f);

                    var _targetGenericPrefabNM = _targetGenericPrefab.GetComponent<NavMeshAgent>();

                    _targetGenericPrefabNM.isStopped = false;

                    _targetGenericPrefabNM.SetDestination(activeGatesList[x].position);

                    //_targetGenericPrefabNM.CalculatePath(activeGatesList[1].position, path);

                    //_targetGenericPrefabNM.SetPath(path);

                }

            }

        }

    }

    public void StartSpawningGenericTargetsEvent()
    { StartCoroutine(StartSpawningGenericTargets()); }


    // Spawn skinny targets.
    public IEnumerator StartSpawningSkinnyTargets()
    {
        float spawnRate = levelStatsSO.skinnyTargetSpawnRate;

        while (true && isSpawningTargets)
        {
            yield return new WaitForSeconds(spawnRate);

            if (isSpawningTargets)
            {
                for (int x = 0; x < targetSpawnTransformList.Count; x++)
                {
                    _targetSkinnyPrefab = Instantiate(targetSkinnySO.targetPrefab, targetSpawnTransformList[x].position, targetSpawnTransformList[x].rotation, null);

                    activeTargetsList.Add(_targetSkinnyPrefab);

                    yield return new WaitForSeconds(0.2f);

                    var _targetSkinnyPrefabNM = _targetSkinnyPrefab.GetComponent<NavMeshAgent>();

                    _targetSkinnyPrefabNM.isStopped = false;

                    _targetSkinnyPrefabNM.SetDestination(activeGatesList[x].position);

                    //_targetSkinnyPrefabNM.CalculatePath(activeGatesList[1].position, path);

                    //_targetSkinnyPrefabNM.SetPath(path);

                }

            }

        }

    }

    public void StartSpawningSkinnyTargetsEvent()
    { StartCoroutine(StartSpawningSkinnyTargets()); }


    // Spawn fat targets.
    public IEnumerator StartSpawningFatTargets()
    {
        float spawnRate = levelStatsSO.fatTargetSpawnRate;

        while (true && isSpawningTargets)
        {
            yield return new WaitForSeconds(spawnRate);

            if (isSpawningTargets)
            {
                for (int x = 0; x < targetSpawnTransformList.Count; x++)
                {
                    _targetFatPrefab = Instantiate(targetFatSO.targetPrefab, targetSpawnTransformList[x].position, targetSpawnTransformList[x].rotation, null);

                    activeTargetsList.Add(_targetFatPrefab);

                    yield return new WaitForSeconds(0.2f);

                    var _targetFatPrefabNM = _targetFatPrefab.GetComponent<NavMeshAgent>();

                    _targetFatPrefabNM.isStopped = false;

                    _targetFatPrefabNM.SetDestination(activeGatesList[x].position);

                    //_targetFatPrefabNM.CalculatePath(activeGatesList[1].position, path);

                    //_targetFatPrefabNM.SetPath(path);

                }

            }

        }

    }

    public void StartSpawningFatTargetsEvent()
    { StartCoroutine(StartSpawningFatTargets()); }


}
