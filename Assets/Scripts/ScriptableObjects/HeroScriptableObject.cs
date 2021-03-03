using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hero Settings")]

public class HeroScriptableObject : ScriptableObject
{
    public GameObject heroPrefab;

    public SkillScriptableObject lightAttackSO;

    public SkillScriptableObject heavyAttackSO;

    public SkillScriptableObject ultimateAttackSO;

    public Vector3 heroStartPos;

    public Vector3 heroStartEuler;

    public float heroStartSize;

}
