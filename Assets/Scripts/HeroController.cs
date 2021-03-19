using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIClass;

public class HeroController : MonoBehaviour
{
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
        firstPersonCamera.transform.LookAt(aimMarble);

        _lightAttackParticleSys.transform.LookAt(aimMarble);

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

        _lightAttackParticleSys = Instantiate(_lightAttackSO.skillGO, particleSystemTransform.position, Quaternion.identity);

        //_heavyAttackParticleSys = Instantiate(_heavyAttackSO.skillGO);

        //_ultimateAttackParticleSys = Instantiate(_ultimateAttackSO.skillGO);

        DisableParticleSystemsOnInstantiate();

    }

    private void DisableParticleSystemsOnInstantiate()
    {
        _lightAttackParticleSys.GetComponentInChildren<ParticleSystem>().Stop();

        //_heavyAttackParticleSys.GetComponent<ParticleSystem>().Stop();

        //_ultimateAttackParticleSys.GetComponent<ParticleSystem>().Stop();

    }

    public void LightAttackShootingState()
    {
        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetLightAttackPressed();

        // StartLightAttackShooting();

    }

    private void StartLightAttackShooting()
    {
        Debug.Log("Entered StartLightShooting");

        isShootingStarted = true;

        Debug.Log(isShootingStarted);

    }

    public void MediumAttackShootingState()
    {
        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetHeavyAttackPressed();

        // StartHeavyAttackShooting();

    }

    private void StartHeavyAttackShooting()
    {
        isShootingStarted = true;

    }

    public void UltimateAttackShootingState()
    {
        _uiManagerSC = FindObjectOfType<UIManager>();

        _uiManagerSC.SetUltimateAttackPressed();

        // StartUltimateAttackShooting();

    }

    private void StartUltimateAttackShooting()
    {
        isShootingStarted = true;

    }

}
