using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;
using UIClass;

public class TargetController : MonoBehaviour
{
    [Header("Ultimate Attack VFX")]

    public VisualEffect ultimateAttackVFX;

    public VisualEffect impactAttackVFX;

    public VisualEffect electricImpactAttackVFX;

    [Header("Stats")]

    [SerializeField] private float health;

    private GameManager _gameManagerSC;

    private Slider healthSlider;

    // Initialize the target controller variables.
    private void Start()
    {
        _gameManagerSC = FindObjectOfType<GameManager>();

        healthSlider = gameObject.GetComponentInChildren<Slider>();

        StartHealthBarSetup();

    }


    // On trigger enter, deal basic attack damage, or
    // add the target to touched heavy attack targets list.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerShank"))
        {
           // Debug.Log("Collision Occurd");

            Vibration.Vibrate(40);

            var damage = _gameManagerSC.lightAttackSO.skillDamage;

            if (impactAttackVFX.isActiveAndEnabled)
            {
                //Debug.Log("Before the play?");

                impactAttackVFX.Play();

                // Debug.Log("After the play?");

            }
            else
            { Debug.Log("Not active and enabled"); }

            UpdateSliderValue(damage);

           // Debug.Log("Minus " + damage + " Damage");

        }
        else if (other.CompareTag("HeavyAttackAOE"))
        {
            _gameManagerSC.touchedTargetsList.Add(this.gameObject);

            Debug.Log("Touched targets list length : " + _gameManagerSC.touchedTargetsList.Count);

        }
        else if (other.CompareTag("EndGate"))
        {
            UIManager.AddStrikeToStrikeCounter();

            _gameManagerSC.activeTargetsList.Remove(gameObject);

            Destroy(gameObject);

        }

    }


    // On trigger exit remove target from touched list.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HeavyAttackAOE"))
        {
            _gameManagerSC.touchedTargetsList.Remove(this.gameObject);

            Debug.Log("Touched targets list length : " + _gameManagerSC.touchedTargetsList.Count);

        }

    }


    // Initialize the max health in the health bar.
    void StartHealthBarSetup()
    {
        healthSlider.maxValue = health;

        healthSlider.value = health;
        
    }


    // Update the health bar value for the hit target.
    public void UpdateSliderValue(float damage)
    {
        health -= damage;

        healthSlider.value = health;

        if (healthSlider.value <= 0)
        {
            _gameManagerSC.activeTargetsList.Remove(gameObject);

            Destroy(gameObject);

        }

    }

    // Initiate ultimate attack effect.
    public void PlayUltimateAttackVFX()
    {
        //Debug.Log("IsActiveAndEnabled?");

        if (ultimateAttackVFX.isActiveAndEnabled)
        {
           //Debug.Log("Before the play?");

            ultimateAttackVFX.Play();

            Vibration.Vibrate(40);

            // Debug.Log("After the play?");

        }
        else
        { Debug.Log("Not active and enabled"); }
        

    }

    public void HeavyAttackVFX()
    {
        electricImpactAttackVFX.Play();

    }

}
