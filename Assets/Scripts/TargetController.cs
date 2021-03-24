using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TargetController : MonoBehaviour
{
    [Header("Stats")]

    [SerializeField] private float health;

    private NavMeshAgent targetNavmesh;

    private Slider healthSlider;

    private void Start()
    {
        targetNavmesh = gameObject.GetComponent<NavMeshAgent>();

        healthSlider = gameObject.GetComponentInChildren<Slider>();

        targetNavmesh.isStopped = true;

        health = 100f;

        StartHealthBarSetup();

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PowerShank"))
        {
            Debug.Log("Collision Occurd");

            var damage = 10f;

            UpdateSliderValue(damage);

            Debug.Log("Minus 10 Damage");

        }

    }

    void StartHealthBarSetup()
    {
        healthSlider.maxValue = health;

        healthSlider.value = health;

    }

    void UpdateSliderValue(float damage)
    {
        health -= damage;

        healthSlider.value = health;

    }    

}
