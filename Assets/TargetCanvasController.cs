using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCanvasController : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.LookRotation(FindObjectOfType<Camera>().transform.position);

    }

}
