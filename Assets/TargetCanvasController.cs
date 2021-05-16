using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCanvasController : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(0f, 11f, -23f));

    }

}
