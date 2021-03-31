using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Stats")]

public class LevelStatsScriptableObject : ScriptableObject
{
    public int levelSecondsTime;

    public int loseConditionNumber;

    public float genericTargetSpawnRate;

    public float skinnyTargetSpawnRate;

    public float fatTargetSpawnRate;

}
