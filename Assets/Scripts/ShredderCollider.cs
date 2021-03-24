using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderCollider : MonoBehaviour
{
    private HeroController _heroControllerSC;

    public void SetupHeroController()
    {
        _heroControllerSC = FindObjectOfType<HeroController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerShank"))
        {
            _heroControllerSC.LightAttackResetSoloProjectile(other.gameObject);

        }

    }

}
