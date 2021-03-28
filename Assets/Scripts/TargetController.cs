using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TargetController : MonoBehaviour
{
    [Header("Stats")]

    [SerializeField] private float health;

    private GameManager _gameManagerSC;

    private NavMeshAgent targetNavmesh;

    private Slider healthSlider;

    private void Start()
    {
        _gameManagerSC = FindObjectOfType<GameManager>();

        targetNavmesh = gameObject.GetComponent<NavMeshAgent>();

        healthSlider = gameObject.GetComponentInChildren<Slider>();

        targetNavmesh.isStopped = true;

        health = 100f;

        StartHealthBarSetup();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerShank"))
        {
            Debug.Log("Collision Occurd");

            var damage = _gameManagerSC.lightAttackSO.skillDamage;

            UpdateSliderValue(damage);

            Debug.Log("Minus " + damage + " Damage");

        }
        else if (other.CompareTag("GroundShutter"))
        {
            _gameManagerSC.touchedTargetsList.Add(this.gameObject);

            Debug.Log("Touched targets list length : " + _gameManagerSC.touchedTargetsList.Count);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GroundShutter"))
        {
            _gameManagerSC.touchedTargetsList.Remove(this.gameObject);

            Debug.Log("Touched targets list length : " + _gameManagerSC.touchedTargetsList.Count);

        }

    }

    void StartHealthBarSetup()
    {
        healthSlider.maxValue = health;

        healthSlider.value = health;

    }

    public void UpdateSliderValue(float damage)
    {
        health -= damage;

        healthSlider.value = health;

    }    

}
