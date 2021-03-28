using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchController : MonoBehaviour
{
    [Header("Public Serialized Variables")]

    [SerializeField] private float cameraFirstPersonStartY;

    [SerializeField] private float cameraFirstPersonStartZ;

    [SerializeField] private float lerpPosSpeed;

    [SerializeField] private float waitBeforeLerpingCamera;

    // Private Variables

    private Vector3 cameraFirstPersonStartPos;

    public void LerpCameraToFirstPersonView()
    {
        Debug.Log("Camera parent : " + transform.parent.name);

        cameraFirstPersonStartPos = new Vector3(0.0f, cameraFirstPersonStartY, cameraFirstPersonStartZ);

        StartCoroutine(LerpCameraToStartPos(cameraFirstPersonStartPos));

    }

    private IEnumerator LerpCameraToStartPos(Vector3 startPos)
    {
        yield return new WaitForSeconds(waitBeforeLerpingCamera);

        var timeSinceStartedPosLerp = 0.0f;

        while (true)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, timeSinceStartedPosLerp * lerpPosSpeed);

            timeSinceStartedPosLerp += Time.deltaTime;

            if (transform.localPosition == startPos)
            {
                yield break;

            }

            yield return null;

        }

    }

}
