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

    private Button heavyAttackButton;

    private GameManager _gameManagerSC;

    // Attack state machine variables.

    private GameObject localheavyAttackAOEGameObject;

    public Transform _lightAttackShootingGO;

    public Transform lightAttackShootingContainer;

    private List<GameObject> _lightAttackPoolList = new List<GameObject>();

    private bool _lightAttackActive;

    private bool _heavyAttackActive;

    private IEnumerator shootingCoroutine;

    private Camera firstPersonCamera;

    private EventSystem _eventSystem;


    // Initializing the class.
    private void Start()
    {
        firstPersonCamera = FindObjectOfType<Camera>();

        Debug.Log(firstPersonCamera.name);

        _eventSystem = FindObjectOfType<EventSystem>();

        _gameManagerSC = FindObjectOfType<GameManager>();

        _lightAttackShootingGO.transform.forward = firstPersonCamera.transform.forward;

        SetupAttacks();

        _lightAttackActive = true;

        _heavyAttackActive = false;

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

                if (touch.phase == TouchPhase.Began && !_eventSystem.IsPointerOverGameObject(touch.fingerId))
                {
                    //Debug.Log("Touch start " + touch.fingerId);

                    if (_lightAttackActive && !_heavyAttackActive)
                    {
                        PlayLightAttack();

                    }
                    else if (!_lightAttackActive && _heavyAttackActive)
                    {
                        localheavyAttackAOEGameObject.SetActive(true);

                    }

                }
                else if (touch.phase == TouchPhase.Moved && !_eventSystem.IsPointerOverGameObject(touch.fingerId))
                {
                    if (_lightAttackActive && !_heavyAttackActive)
                    {
                        _lightAttackShootingGO.transform.eulerAngles =
                                           new Vector3(firstPersonCamera.transform.eulerAngles.x + upProjectileOffset,
                                                       firstPersonCamera.transform.eulerAngles.y - leftProjectileOffset, 0f);

                    }
                    else if (!_lightAttackActive && _heavyAttackActive)
                    {
                        StopLightAttack();

                        firstPersonCamera.GetComponent<CameraTouchController>().cameraAimSpeed = 1.5f;

                        _lightAttackShootingGO.transform.eulerAngles =
                                           new Vector3(firstPersonCamera.transform.eulerAngles.x + upProjectileOffset,
                                                       firstPersonCamera.transform.eulerAngles.y - leftProjectileOffset, 0f);

                        if (localheavyAttackAOEGameObject.activeSelf)
                        {
                            RaycastHeavyAttackDome(localheavyAttackAOEGameObject);

                        }
                        else if (!localheavyAttackAOEGameObject.activeSelf)
                        {
                            localheavyAttackAOEGameObject.SetActive(true);

                        }

                    }

                }
                else if (touch.phase == TouchPhase.Ended && !IsTouchOnUIElement(touch.position))
                {
                    //Debug.Log("Touch ended " + touch.fingerId);

                    //Debug.Log("Touch count " + Input.touchCount);

                    if (_lightAttackActive && !_heavyAttackActive)
                    {
                        StopLightAttack();

                    }
                    else if (!_lightAttackActive && _heavyAttackActive)
                    {
                        ShootHeavyAttack();

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

        heavyAttackButton = _uiManagerSC.heavyAttackButton;

        heavyAttackButton.onClick.AddListener(HeavyAttackShootingState);

        //Debug.Log("ui manager type : " + _uiManagerSC.name);

        _lightAttackSO = _uiManagerSC.lightAttackSO;

        _heavyAttackSO = _uiManagerSC.heavyAttackSO;

        _ultimateAttackSO = _uiManagerSC.ultimateAttackSO;

        _lightAttackGO = _lightAttackSO.skillGO;

        for (int x = 0; x < 40; x++)
        {
            var instance = Instantiate(_lightAttackGO, _lightAttackShootingGO.position, Quaternion.identity, lightAttackShootingContainer);

            instance.transform.forward = _lightAttackShootingGO.transform.forward;

            _lightAttackPoolList.Add(instance);

            instance.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            instance.GetComponent<Rigidbody>().isKinematic = true;

            instance.SetActive(false);

        }

        shootingCoroutine = LightAttackShootingCoroutine(_lightAttackSO.skillCooldown);

        localheavyAttackAOEGameObject = Instantiate(_heavyAttackSO.skillGO);

        localheavyAttackAOEGameObject.SetActive(false);

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

    }


    // Reset a projectile that touched the shredder collider.
    // This is the function that resets objects for the object pooling system.
    public void LightAttackResetSoloProjectile(GameObject projectile)
    {
        projectile.SetActive(false);

        projectile.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        projectile.GetComponent<Rigidbody>().isKinematic = true;

        projectile.transform.localPosition = Vector3.zero;

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

            }

            if (x == _lightAttackPoolList.Count - 1)
            {
                x = 0;

            }

            yield return new WaitForSeconds(cooldown);

        }

    }


    // Add physical force the the projectile (shoot it).
    private void LightAttackAddProjectileForce(GameObject go)
    {
        go.GetComponent<Rigidbody>().mass = _lightAttackSO.skillMass;

        go.GetComponent<Rigidbody>().velocity = _lightAttackShootingGO.forward * _lightAttackSO.skillSpeed;

        go.transform.localRotation = _lightAttackShootingGO.localRotation;

    }


    // ~~ Heavy attack functions. ~~

    // Invoke heavy attack event, press down heavy attack button.
    public void HeavyAttackShootingState()
    {
        _heavyAttackActive = true;
        _lightAttackActive = false;

        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetHeavyAttackPressed();

    }

    // Do the actual damage to the targets that are
    // touching the heavy attack aim dome.
    public void ShootHeavyAttack()
    {
        StartCoroutine(_gameManagerSC.TargetsListHeavyDamage());

        StartCoroutine(HeavyAttackTimerCoroutine());

        firstPersonCamera.GetComponent<CameraTouchController>().cameraAimSpeed = 2.2f;

        _heavyAttackActive = false;
        _lightAttackActive = true;

    }

    private void RaycastHeavyAttackDome(GameObject electricSphere)
    {
        RaycastHit hit;

        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out hit))
        {
            //Debug.Log("Raycast Hit : " + hit.collider.name);

            //Debug.Log("Raycast Hit Point : " + hit.point);

            //Debug.Log("localheavyAttackAOE : " + electricSphere.name);

            electricSphere.transform.position = new Vector3(hit.point.x, -4.5f, hit.point.z);

        }

    }


    private IEnumerator HeavyAttackTimerCoroutine()
    {
        for (int i = 0; i < _heavyAttackSO.skillDuration; i++)
        {
            yield return new WaitForSeconds(1);

            i++;

        }

        _gameManagerSC.heavyAttackTimeOver = true;

        localheavyAttackAOEGameObject.SetActive(false);

        StartCoroutine(_uiManagerSC.SetHeavyAttackCooldown());

        yield break;

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
