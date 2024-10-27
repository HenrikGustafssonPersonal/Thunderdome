using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float burstSpeed = 0.2f;
    public float retreatPercent = 0.2f;
    public Transform repairingPos;
    public Transform repairBeamTarget;

    [SerializeField]
    private GameObject healingParticles;

    [SerializeField]
    private GameObject damagedParticles;

    public void setDamagedParticles(bool turnOn)
    {
        damagedParticles.SetActive(turnOn);
    }

    public void setHealingParticles(bool turnOn)
    {
        healingParticles.SetActive(turnOn);
    }

    public override void TakeDamge(float amout)
    {
        base.TakeDamge(amout);

        setDamagedParticles(currentHealth <= maxHealth * retreatPercent && currentHealth > 0.0f);

    }
}
