using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchController : MonoBehaviour
{
    [Header("Public Serialized Variables")]

    [SerializeField] private float cameraFirstPersonStartY;

    [SerializeField] private float cameraFirstPersonStartZ;

    [SerializeField] private float cameraFirstPersonStartRotX;

    [SerializeField] private float cameraFirstPersonStartRotY;

    [SerializeField] private float lerpRotSpeed;

    [SerializeField] private float lerpPosSpeed;

    [SerializeField] private float waitBeforeLerpingCamera;

    // Private Variables

    private Vector3 cameraFirstPersonStartPos;

    private Quaternion cameraFirstPersonStartRot;

    public void LerpCameraToFirstPersonView()
    {
        Debug.Log("Camera parent : " + transform.parent.name);

        cameraFirstPersonStartPos = new Vector3(0.0f, cameraFirstPersonStartY, cameraFirstPersonStartZ);

        cameraFirstPersonStartRot.eulerAngles = new Vector3(cameraFirstPersonStartRotX, cameraFirstPersonStartRotY, 0.0f);

        StartCoroutine(LerpCameraToStartPos(cameraFirstPersonStartPos));

        StartCoroutine(LerpCameraToStartRot(cameraFirstPersonStartRot));

    }

    private IEnumerator LerpCameraToStartRot(Quaternion startRot)
    {
        yield return new WaitForSeconds(waitBeforeLerpingCamera);

        var timeSinceStartedRotLerp = 0.0f;

        while (true)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, startRot, timeSinceStartedRotLerp * lerpRotSpeed);

            timeSinceStartedRotLerp += Time.deltaTime;

            if (transform.localRotation == startRot)
            {
                yield break;

            }

            yield return null;

        }

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
