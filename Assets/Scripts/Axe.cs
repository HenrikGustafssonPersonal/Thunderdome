using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Axe : BaseGun
{
    public float preCooldown = 1f;

    public override void Update()
    {
        fire1Cooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1") && fire1Cooldown <= 0)
        {
            fire1Cooldown = 1 / fireRate;
            AttackAnimation(fire1Cooldown);
            StartSwing();
        }
    }

    private void StartSwing()
    {
        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, preCooldown).OnComplete(() =>
        {

            Shoot();


        });
    }
}
