using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchController : MonoBehaviour
{
    [Header("Public Serialized Variables")]

    [SerializeField] private float cameraFirstPersonStartY;

    [SerializeField] private float cameraFirstPersonStartZ;

    [SerializeField] private float lerpPosSpeed;

    [SerializeField] private float waitBeforeLerpingCamera;

    [SerializeField] private StateManager stateManagerCS;

    [Header("Camera Swiping Settings")]

    public float cameraAimSpeed = 0.5f;

    [SerializeField] private float direction = -1;

    [SerializeField] private float cameraZoomSpeed;

    [SerializeField] private int upDownClampAngleMin;

    [SerializeField] private int upDownClampAngleMax;

    [SerializeField] private int leftRightClampAngle;

    [SerializeField] private LayerMask ignoreMask;

    // Private Variables

    private Coroutine zoomInCoroutine;

    private Coroutine zoomOutCoroutine;

    private EventSystem _eventSystem;

    private Vector3 cameraFirstPersonStartPos;

    private Camera thisCamera;

    private Vector3 originalRot;

    private float rotX = 0f;

    private float rotY = 0f;

    public float strengthFactor = 0.0001f;

    public void Start()
    {
        if (gameObject.tag != CameraPicker.cameraPick.ToString())
        {
            gameObject.SetActive(false);

        }

        _eventSystem = FindObjectOfType<EventSystem>();

        stateManagerCS.GameLoadedEvent.AddListener(LerpCameraToFirstPersonView);

        thisCamera = gameObject.GetComponent<Camera>();

        originalRot = thisCamera.transform.rotation.eulerAngles;

        rotY = originalRot.y;

        rotX = originalRot.x;

        //Debug.Log("rotX : " + rotX);

        //Debug.Log("rotY : " + rotY);

    }

    // In fixed update, we listen to a touches from the user.
    // The user can swipe to rotate the camera.
    private void Update()
    {
        if (Input.touchCount > 0 && Input.touchCount < 3)
        {
            // EventSystem.current.IsPointerOverGameObject(touch.fingerId) &&
            // EventSystem.current.currentSelectedGameObject.GetComponent<CanvasRenderer>() != null

            for (int x = 0; x < Input.touchCount; x++)
            {
                var touch = Input.touches[x];

                if (touch.phase == TouchPhase.Began && !IsTouchOnUIElement(touch.position))
                {
                    Debug.Log("Touch start " + touch.fingerId);

                    zoomInCoroutine = StartCoroutine(ZoomInCameraWhileShooting());

                    // aimCrosshair.SetActive(true);

                    // PlayLightAttack();

                }
                else if (touch.phase == TouchPhase.Moved && !IsTouchOnUIElement(touch.position))
                {
                    var moveSpeed = touch.deltaPosition.magnitude / Time.deltaTime;

                   // Debug.Log("Move Speed Camera Before Tweaking : " + moveSpeed);

                    var factor = strengthFactor * moveSpeed;

                   // Debug.Log("Move Speed Camera After Tweaking : " + factor);

                    rotX -= touch.deltaPosition.y * Time.deltaTime * (cameraAimSpeed + factor) * direction;

                    rotY += touch.deltaPosition.x * Time.deltaTime * (cameraAimSpeed + factor) * direction ;

                    // Debug.Log(" rotX : " + rotX + " rotY : " + rotY);

                    rotY = Mathf.Clamp(rotY, -leftRightClampAngle, leftRightClampAngle);

                    rotX = Mathf.Clamp(rotX, upDownClampAngleMin, upDownClampAngleMax);

                    thisCamera.transform.eulerAngles = new Vector3(rotX, rotY, 0f);

                    // Debug.Log(" firstPersonCamera.transform.eulerAngles (Before) : " + firstPersonCamera.transform.eulerAngles);

                    // Debug.Log(" firstPersonCamera.transform.eulerAngles (After) : " + firstPersonCamera.transform.eulerAngles);

                    // _lightAttackShootingGO.transform.eulerAngles = new Vector3(thisCamera.transform.eulerAngles.x + upProjectileOffset, firstPersonCamera.transform.eulerAngles.y - leftProjectileOffset, 0f);

                }
                else if (touch.phase == TouchPhase.Ended && !IsTouchOnUIElement(touch.position))
                {
                    //Debug.Log("Touch ended " + touch.fingerId);

                    //Debug.Log("Touch count " + Input.touchCount);

                    zoomOutCoroutine = StartCoroutine(ZoomOutCameraWhileNotShooting());

                    // StartCoroutine(StopLightAttack());

                }

            }

        }

    }

    //// Countdown coroutine that get's cancelled if the user clicks the screen.
    //// If the user hasn't clicked for 4 seconds, zoom out camera.
    //private IEnumerator ZoomOutCameraCountdownTimer()
    //{
    //    zoomCameraBool = true;

    //    for (int x = 0; x < 1; x++)
    //    {
    //        Debug.Log(x);

    //        yield return new WaitForSeconds(0.2f);

    //        if (!zoomCameraBool)
    //        {
    //            yield break;

    //        }

    //    }

    //    StartCoroutine(ZoomOutCameraWhileNotShooting());

    //}

    // A function that checks if a touch is on ui element. (Universal function)
    private bool IsTouchOnUIElement(Vector3 touchPos)
    {
        bool isOnUI = true;

        PointerEventData ped = new PointerEventData(EventSystem.current);

        ped.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(ped, results);

        if (results.Count == 0)
        {
            isOnUI = false;

        }
        else
        {
            isOnUI = true;
        }

        //Debug.Log("Is touch on ui? " + isOnUI.ToString());

        return isOnUI;

    }


    // Zoom out the camera field of view, while the player is not aiming. 
    // Starts after a small wait time.
    private IEnumerator ZoomOutCameraWhileNotShooting()
    {
        if (zoomInCoroutine != null)
        {
            StopCoroutine(zoomInCoroutine);

            zoomInCoroutine = null;

        }

        var timeSinceStartedZoomOut = 0.0f;

        while (true)
        {
            thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, 40f, timeSinceStartedZoomOut * cameraZoomSpeed);

            timeSinceStartedZoomOut += Time.deltaTime;

            if (thisCamera.fieldOfView == 40f)
            {
                yield break;

            }

            yield return null;

        }

    }


    // Zoom in the camera field of view, while the player is shooting.
    private IEnumerator ZoomInCameraWhileShooting()
    {
        if (zoomOutCoroutine != null)
        {
            StopCoroutine(zoomOutCoroutine);

            zoomOutCoroutine = null;

        }

        var timeSinceStartedZoomIn = 0.0f;

        while (true)
        {
            thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, 35f, timeSinceStartedZoomIn * cameraZoomSpeed);

            timeSinceStartedZoomIn += Time.deltaTime;

            if (thisCamera.fieldOfView == 35f)
            {
                yield break;

            }

            yield return null;

        }

    }

    public void LerpCameraToFirstPersonView()
    {
        //Debug.Log("Camera parent : " + transform.parent.name);

        if (gameObject.activeSelf)
        {
            cameraFirstPersonStartPos = new Vector3(0.0f, cameraFirstPersonStartY, cameraFirstPersonStartZ);

            StartCoroutine(LerpCameraToStartPos(cameraFirstPersonStartPos));

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
