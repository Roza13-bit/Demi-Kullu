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

    // Private Variables.

    // Attack scriptable objects.
    private SkillScriptableObject _lightAttackSO;
    private GameObject _lightAttackParticleSys;

    private SkillScriptableObject _heavyAttackSO;
    private GameObject _heavyAttackParticleSys;

    private SkillScriptableObject _ultimateAttackSO;
    private GameObject _ultimateAttackParticleSys;

    private UIManager _uiManagerSC;

    // Attack state machine variables.
    private Transform particleSystemTransform;

    private bool isShootingStarted;

    private Transform aimMarble;

    private CameraTouchController firstPersonCamera;

    private void Start()
    {
        Debug.Log("Hero Controller Has Started.");

        isShootingStarted = true;

        particleSystemTransform = transform.Find("LightAttackParticleTransform");

        aimMarble = FindObjectOfType<aimController>().transform;

        firstPersonCamera = FindObjectOfType<CameraTouchController>();

        SetupAttacks();

    }

    private void FixedUpdate()
    {
        if (isShootingStarted)
        {
            RotateCameraAndAim();

        }

    }

    private void RotateCameraAndAim()
    {
        firstPersonCamera.transform.LookAt(aimMarble, Vector3.up);

        _lightAttackParticleSys.transform.LookAt(aimMarble, Vector3.up);

        //_heavyAttackParticleSys.transform.LookAt(aimMarble);

        //_ultimateAttackParticleSys.transform.LookAt(aimMarble);

    }

    private void SetupAttacks()
    {
        _uiManagerSC = FindObjectOfType<UIManager>();

        Debug.Log("ui manager type : " + _uiManagerSC.name);

        _lightAttackSO = _uiManagerSC.lightAttackSO;

        _heavyAttackSO = _uiManagerSC.heavyAttackSO;

        _ultimateAttackSO = _uiManagerSC.ultimateAttackSO;

        Debug.Log("Light attack particle system name : " + _lightAttackSO.skillGO.name);

        _lightAttackParticleSys = Instantiate(_lightAttackSO.skillGO, particleSystemTransform.position, Quaternion.identity, transform);

        //_heavyAttackParticleSys = Instantiate(_heavyAttackSO.skillGO);

        //_ultimateAttackParticleSys = Instantiate(_ultimateAttackSO.skillGO);

    }

    public void LightAttackShootingState()
    {
        LightAttackEvent.Invoke();

        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetLightAttackPressed();

    }

    public void PlayLightAttack()
    {
        LightAttackParticleController lightParticleSys = FindObjectOfType<LightAttackParticleController>();

        lightParticleSys.PlayLightAttack();

    }

    public void StopLightAttack()
    {
        LightAttackParticleController lightParticleSys = FindObjectOfType<LightAttackParticleController>();

        lightParticleSys.StopLightAttack();

    }

    public void HeavyAttackShootingState()
    {
        HeavyAttackEvent.Invoke();

        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetHeavyAttackPressed();

    }

    public void UltimateAttackShootingState()
    {
        UltimateAttackEvent.Invoke();

        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetUltimateAttackPressed();

    }

}
