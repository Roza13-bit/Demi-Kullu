using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    [SerializeField] private LayerMask ignoreMask;

    // ~~ Private Variables. ~~

    // Attack scriptable objects.
    private SkillScriptableObject _lightAttackSO;
    private GameObject _lightAttackGO;

    private SkillScriptableObject _heavyAttackSO;
    private GameObject _heavyAttackGO;

    private SkillScriptableObject _ultimateAttackSO;
    private GameObject _ultimateAttackGO;

    private UIManager _uiManagerSC;

    private GameManager _gameManagerSC;

    // Attack state machine variables.

    private Transform _lightAttackShootingGO;

    private List<GameObject> _lightAttackPoolList = new List<GameObject>();

    static bool _lightAttackActive;

    [SerializeField] private Transform _heavyAttackAimDomeGO;

    private IEnumerator shootingCoroutine;

    private IEnumerator zoomInCoroutine;

    private IEnumerator zoomOutCoroutine;

    // Camera swiping variables.

    private Camera firstPersonCamera;

    private float rotX = 0f;

    private float rotY = 0f;

    private Vector3 originalRot;

    private GameObject aimCrosshair;

    private EventSystem _eventSystem;

    private bool zoomCameraBool;

    // Initializing the class.
    private void Start()
    {
        _lightAttackShootingGO = transform.Find("LightAttackTransformGO");

        firstPersonCamera = FindObjectOfType<Camera>();

        _eventSystem = FindObjectOfType<EventSystem>();

        _gameManagerSC = FindObjectOfType<GameManager>();

        _lightAttackShootingGO.transform.forward = firstPersonCamera.transform.forward;

        SetupAttacks();

        originalRot = firstPersonCamera.transform.eulerAngles;

        rotY = originalRot.y;

        rotX = originalRot.x;

        Debug.Log("rotX : " + rotX);

        Debug.Log("rotY : " + rotY);

        aimCrosshair = _uiManagerSC.aimCrosshair;

        _lightAttackActive = true;

    }


    // In fixed update, we listen to a touches from the user.
    // The user can swipe to rotate the camera.
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            // EventSystem.current.IsPointerOverGameObject(touch.fingerId) &&
            // EventSystem.current.currentSelectedGameObject.GetComponent<CanvasRenderer>() != null

            for (int x = 0; x < 1; x++)
            {
                var touch = Input.touches[x];

                if (touch.phase == TouchPhase.Began && !_eventSystem.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("Touch start " + touch.fingerId);

                    StartCoroutine(ZoomInCameraWhileShooting());

                    aimCrosshair.SetActive(true);

                    PlayLightAttack();

                }
                else if (touch.phase == TouchPhase.Moved && !_eventSystem.IsPointerOverGameObject(touch.fingerId))
                {
                    rotX -= touch.deltaPosition.y * Time.deltaTime * cameraAimSpeed * direction;

                    rotY += touch.deltaPosition.x * Time.deltaTime * cameraAimSpeed * direction;

                    // Debug.Log(" rotX : " + rotX + " rotY : " + rotY);

                    rotY = Mathf.Clamp(rotY, -leftRightClampAngle, leftRightClampAngle);

                    rotX = Mathf.Clamp(rotX, upDownClampAngleMin, upDownClampAngleMax);

                    // Debug.Log(" firstPersonCamera.transform.eulerAngles (Before) : " + firstPersonCamera.transform.eulerAngles);

                    firstPersonCamera.transform.eulerAngles = new Vector3(rotX, rotY, 0f);

                    // Debug.Log(" firstPersonCamera.transform.eulerAngles (After) : " + firstPersonCamera.transform.eulerAngles);

                    _lightAttackShootingGO.transform.eulerAngles = new Vector3(firstPersonCamera.transform.eulerAngles.x + upProjectileOffset, firstPersonCamera.transform.eulerAngles.y - leftProjectileOffset, 0f);

                }
                else if (touch.phase == TouchPhase.Ended && !IsTouchOnUIElement(touch.position))
                {
                    Debug.Log("Touch ended " + touch.fingerId);

                    Debug.Log("Touch count " + Input.touchCount);

                    if (!_lightAttackActive)
                    {
                        ShootHeavyAttack();

                        StartCoroutine(ZoomOutCameraCountdownTimer());

                    }
                    else if (_lightAttackActive)
                    {
                        StopLightAttack();

                        StartCoroutine(ZoomOutCameraCountdownTimer());

                    }

                }

            }

        }

    }


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

        Debug.Log("Is touch on ui? " + isOnUI.ToString());

        return isOnUI;

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

        shootingCoroutine = LightAttackShootingCoroutine(_lightAttackSO.skillCooldown);

    }


    // Countdown coroutine that get's cancelled if the user clicks the screen.
    // If the user hasn't clicked for 4 seconds, zoom out camera.
    private IEnumerator ZoomOutCameraCountdownTimer()
    {
        zoomCameraBool = true;

        for (int x = 0; x < 4; x++)
        {
            Debug.Log(x);

            yield return new WaitForSeconds(1);

            if (!zoomCameraBool)
            {
                yield break;

            }

        }

        StartCoroutine(ZoomOutCameraWhileNotShooting());

    }


    // Zoom out the camera field of view, while the player is not aiming. 
    // Starts after a small wait time.
    private IEnumerator ZoomOutCameraWhileNotShooting()
    {
        var timeSinceStartedZoomOut = 0.0f;

        while (true)
        {
            firstPersonCamera.fieldOfView = Mathf.Lerp(firstPersonCamera.fieldOfView, 50f, timeSinceStartedZoomOut * cameraZoomSpeed);

            timeSinceStartedZoomOut += Time.deltaTime;

            if (firstPersonCamera.fieldOfView == 50f)
            {
                yield break;

            }

            yield return null;

        }

    }


    // Zoom in the camera field of view, while the player is shooting.
    private IEnumerator ZoomInCameraWhileShooting()
    {
        zoomCameraBool = false;

        var timeSinceStartedZoomIn = 0.0f;

        while (true)
        {
            firstPersonCamera.fieldOfView = Mathf.Lerp(firstPersonCamera.fieldOfView, 35f, timeSinceStartedZoomIn * cameraZoomSpeed);

            timeSinceStartedZoomIn += Time.deltaTime;

            if (firstPersonCamera.fieldOfView == 35f)
            {
                yield break;

            }

            yield return null;

        }

    }


            // ~~ Light attack functions. ~~

    // Start shooting the light attack.
    public void PlayLightAttack()
    {
        StartCoroutine(shootingCoroutine);

    }


    // Stop and reset the light attack.
    public void StopLightAttack()
    {
        StopCoroutine(shootingCoroutine);

        foreach (GameObject go in _lightAttackPoolList)
        {
            go.SetActive(false);

            go.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            go.GetComponent<Rigidbody>().isKinematic = true;

            go.transform.localPosition = Vector3.zero;

        }

        aimCrosshair.SetActive(false);

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
            if (_lightAttackActive)
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
            else if (!_lightAttackActive)
            {
                Ray ray = new Ray();

                RaycastHit hit;

                ray.origin = firstPersonCamera.transform.position;

                ray.direction = firstPersonCamera.transform.forward;

                Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 1f);

                if (Physics.Raycast(ray, out hit, 200f, ~ignoreMask))
                {
                    if (hit.collider.CompareTag("FightGround"))
                    {
                        _heavyAttackAimDomeGO.position = new Vector3(hit.point.x, -5f, hit.point.z);

                    }

                }
                    
                x = 0;

                yield return new WaitForFixedUpdate();

            }

        }

    }


    // Add physical force the the projectile (shoot it).
    private void LightAttackAddProjectileForce(GameObject go)
    {
        go.GetComponent<Rigidbody>().mass = _lightAttackSO.skillMass;

        go.GetComponent<Rigidbody>().velocity = _lightAttackShootingGO.forward * _lightAttackSO.skillSpeed;

    }


            // ~~ Heavy attack functions. ~~

    // Invoke heavy attack event, press down heavy attack button.
    public void HeavyAttackShootingState()
    {
        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetHeavyAttackPressed();

        HeavyAttackEvent.Invoke();

    }


    // Change the shooting state to heavy attack mode.
    public void StartHeavyAttackAimAndShoot()
    {
        Debug.Log("Heavy event");

        _lightAttackActive = false;

    }


    // Do the actual damage to the targets that are
    // touching the heavy attack aim dome.
    private void ShootHeavyAttack()
    {
        StopCoroutine(shootingCoroutine);

        _lightAttackActive = true;

        _gameManagerSC.TargetsListHeavyDamage();

        _heavyAttackAimDomeGO.localPosition = new Vector3(0f, 0f, 400f);

        StartCoroutine(_uiManagerSC.SetHeavyAttackCooldown());

        foreach (GameObject go in _lightAttackPoolList)
        {
            go.SetActive(false);

            go.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            go.GetComponent<Rigidbody>().isKinematic = true;

            go.transform.localPosition = Vector3.zero;

        }

    }


            // ~~ Ultimate attack functions. ~~

    // Set ultimate attack button to pressed.
    // Invoke ultimate attack event.
    public void UltimateAttackShootingState()
    {
        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetUltimateAttackPressed();

        UltimateAttackEvent.Invoke();

    }


    // Shoot ultimate attack.
    // Start ultimate attack cd.
    public void ShootUltimateAttack()
    {
        _uiManagerSC.StartUltimateCDTimer();

        _gameManagerSC = FindObjectOfType<GameManager>();

        _gameManagerSC.TargetsListUltimateDamage();

    }


}
