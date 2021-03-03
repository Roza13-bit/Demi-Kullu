using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tap To Fire Controller Settings")]

public class TapToFireControllerScriptableObject : ScriptableObject
{
    public float projectileSpeed;

    public float projectileMass;

    public float projectileSizeMultiply;

}
