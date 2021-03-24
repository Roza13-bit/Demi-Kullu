using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UIClass;

public class HeroController : MonoBehaviour
{
    [Header("Attack States")]

    public UnityEvent LightAttackEvent;

    public UnityEvent HeavyAttackEvent;

    public UnityEvent UltimateAttackEvent;

    [Header("Camera Swiping Settings")]

    public float cameraAimSpeed = 0.5f;

    [SerializeField] private float direction = -1;

    [SerializeField] private float cameraZoomSpeed;

    [SerializeField] private int upDownClampAngleMin;

    [SerializeField] private int upDownClampAngleMax;

    [SerializeField] private int leftRightClampAngle;

    [SerializeField] private float leftProjectileOffset;

    [SerializeField] private float upProjectileOffset;

    // ~~ Private Variables. ~~

    // Attack scriptable objects.
    private SkillScriptableObject _lightAttackSO;
    private GameObject _lightAttackGO;

    private SkillScriptableObject _heavyAttackSO;
    private GameObject _heavyAttackGO;

    private SkillScriptableObject _ultimateAttackSO;
    private GameObject _ultimateAttackGO;

    private UIManager _uiManagerSC;

    // Attack state machine variables.

    private Transform _lightAttackShootingGO;

    private List<GameObject> _lightAttackPoolList = new List<GameObject>();

    // Camera swiping variables.

    private Touch initTouch = new Touch();

    private Camera firstPersonCamera;

    private float rotX = 0f;

    private float rotY = 0f;

    private Vector3 originalRot;

    private GameObject aimCrosshair;

    // Initializing the class.
    private void Start()
    {
        _lightAttackShootingGO = transform.Find("LightAttackTransformGO");

        firstPersonCamera = FindObjectOfType<Camera>();

        _lightAttackShootingGO.transform.forward = firstPersonCamera.transform.forward;

        SetupAttacks();

        originalRot = firstPersonCamera.transform.eulerAngles;

        rotX = originalRot.x;

        rotY = originalRot.y;

        aimCrosshair = _uiManagerSC.aimCrosshair;

    }

    // In fixed update, we listen to a touches from the user.
    // The user can swipe to rotate the camera.
    private void FixedUpdate()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                initTouch = touch;

                StartCoroutine(ZoomInCameraWhileShooting());

                aimCrosshair.SetActive(true);

                PlayLightAttack();

            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = initTouch.position.x - touch.position.x;

                float deltaY = initTouch.position.y - touch.position.y;

                rotX -= deltaX * Time.deltaTime * cameraAimSpeed * direction;

                rotY += deltaY * Time.deltaTime * cameraAimSpeed * direction;

                rotX = Mathf.Clamp(rotX, -leftRightClampAngle, leftRightClampAngle);

                rotY = Mathf.Clamp(rotY, upDownClampAngleMin, upDownClampAngleMax);
                
                firstPersonCamera.transform.eulerAngles = new Vector3(rotY, rotX, 0f);

                _lightAttackShootingGO.transform.eulerAngles = new Vector3(firstPersonCamera.transform.eulerAngles.x + upProjectileOffset, firstPersonCamera.transform.eulerAngles.y - leftProjectileOffset, 0f);

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                StopLightAttack();

                aimCrosshair.SetActive(false);

                StartCoroutine(ZoomOutCameraWhileNotShooting());

                initTouch = new Touch();

            }

        }

    }

    // Draw the scriptable attack scripts from the UIManager to local variables.
    private void SetupAttacks()
    {
        _uiManagerSC = FindObjectOfType<UIManager>();

        Debug.Log("ui manager type : " + _uiManagerSC.name);

        _lightAttackSO = _uiManagerSC.lightAttackSO;

        _heavyAttackSO = _uiManagerSC.heavyAttackSO;

        _ultimateAttackSO = _uiManagerSC.ultimateAttackSO;

        _lightAttackGO = _lightAttackSO.skillGO;

        for (int x = 0; x < 15; x++)
        {
            var instance = Instantiate(_lightAttackGO, _lightAttackShootingGO.position, Quaternion.identity, _lightAttackShootingGO);

            instance.transform.forward = _lightAttackShootingGO.transform.forward;

            _lightAttackPoolList.Add(instance);

            instance.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            instance.GetComponent<Rigidbody>().isKinematic = true;

            instance.SetActive(false);

        }

    }

    // Zoom out the camera field of view, while the player is not aiming. 
    // Starts after a small wait time.
    private IEnumerator ZoomOutCameraWhileNotShooting()
    {
        StopCoroutine(ZoomInCameraWhileShooting());

        var timeSinceStartedZoomOut = 0.0f;

        while (true)
        {
            firstPersonCamera.fieldOfView = Mathf.Lerp(firstPersonCamera.fieldOfView, 50f, timeSinceStartedZoomOut * cameraZoomSpeed);

            timeSinceStartedZoomOut += Time.deltaTime;

            if (firstPersonCamera.fieldOfView == 50f)
            {
                yield break;

            }

            yield return new WaitForFixedUpdate();

        }

    }

    // Zoom in the camera field of view, while the player is shooting.
    private IEnumerator ZoomInCameraWhileShooting()
    {
        StopCoroutine(ZoomOutCameraWhileNotShooting());

        var timeSinceStartedZoomIn = 0.0f;

        while (true)
        {
            firstPersonCamera.fieldOfView = Mathf.Lerp(firstPersonCamera.fieldOfView, 35f, timeSinceStartedZoomIn * cameraZoomSpeed);

            timeSinceStartedZoomIn += Time.deltaTime;

            if (firstPersonCamera.fieldOfView == 35f)
            {
                yield break;

            }

            yield return new WaitForFixedUpdate();

        }

    }

    // ~~ Light attack functions. ~~

    // Start shooting the light attack.
    public void PlayLightAttack()
    {
        StartCoroutine(LightAttackShootingCoroutine(_lightAttackSO.skillCooldown));

    }

    // Stop and reset the light attack.
    public void StopLightAttack()
    {
        StopAllCoroutines();

        foreach (GameObject go in _lightAttackPoolList)
        {
            go.SetActive(false);

            go.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            go.GetComponent<Rigidbody>().isKinematic = true;

            go.transform.localPosition = Vector3.zero;

        }

    }

    // Reset a projectile that touched the shredder collider.
    // This is the function that resets objects for the object pooling system.
    public void LightAttackResetSoloProjectile(GameObject projectile)
    {
        for (int x = 0; x < _lightAttackPoolList.Count; x++)
        {
            if (_lightAttackPoolList[x] == projectile)
            {
                projectile.SetActive(false);

                projectile.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                projectile.GetComponent<Rigidbody>().isKinematic = true;

                projectile.transform.localPosition = Vector3.zero;

            }

        }

    }

    // Activate the projectile shooting loop.
    // This function waits the cooldown time, and than activates and shoots
    // a projectile from the object pool.
    private IEnumerator LightAttackShootingCoroutine(float cooldown)
    {
        for (int x = 0; x < _lightAttackPoolList.Count; x++)
        {
            if (!_lightAttackPoolList[x].activeSelf)
            {
                var instance = _lightAttackPoolList[x];

                instance.SetActive(true);

                instance.GetComponent<Rigidbody>().isKinematic = false;

                instance.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                LightAttackAddProjectileForce(instance);

                x = 0;

            }

            yield return new WaitForSeconds(cooldown);

        }

    }

    // Add physical force the the projectile (shoot it).
    private void LightAttackAddProjectileForce(GameObject go)
    {
        go.GetComponent<Rigidbody>().mass = _lightAttackSO.skillMass;

        go.GetComponent<Rigidbody>().velocity =  _lightAttackShootingGO.forward * _lightAttackSO.skillSpeed;

    }

    // ~~ Heavy attack functions. ~~

    public void HeavyAttackShootingState()
    {
        HeavyAttackEvent.Invoke();

        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetHeavyAttackPressed();

    }

    // ~~ Ultimate attack functions. ~~

    public void UltimateAttackShootingState()
    {
        UltimateAttackEvent.Invoke();

        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetUltimateAttackPressed();

    }

}
