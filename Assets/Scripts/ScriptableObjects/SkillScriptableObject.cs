using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill Settings")]

public class SkillScriptableObject : ScriptableObject
{
    public GameObject skillGO;

    public Sprite skillSpriteMain;

    public Sprite skillSpritePressed;

    public float skillDamage;

    public float skillCooldown;

    public float skillSpeed;

    public float skillMass;

    public float skillDuration;

}
