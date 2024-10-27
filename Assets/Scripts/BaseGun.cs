using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BaseGun : MonoBehaviour
{
    public float damage = 10.0f;
    public float range = 100f;
    public float fireRate = 15f;
    public float specialFireRate = .1f;
    public float hitForce = 300f;

    public LayerMask layerIgnore = 7; 

    public Camera fpsCam;
    
    public GameObject enviormentHitParticles;
    public GameObject enemyHitParticles;

    protected float fire1Cooldown = 0f;
    public float fire2Cooldown = 0f;
    protected Animator anim;

    private float entryCooldown = 0.7f;
    protected float currentEntryCooldown;



    public void Start()
    {
        ResetEntryCooldown();
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(currentEntryCooldown >= 0)
        {
            currentEntryCooldown -= Time.deltaTime;
            return;
        }

        // Normal attack:
        fire1Cooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1") && fire1Cooldown <= 0)
        {
            fire1Cooldown = 1.0f / fireRate;
            Shoot();
            AttackAnimation(fire1Cooldown);
        }

        ////Special attack:
        ////fire2Cooldown -= Time.deltaTime;
        //GetComponentInParent<PlayerStats>().curre
        //if (Input.GetButton("Fire2") && fire2Cooldown <= 0)
        //{
        //    fire2Cooldown = 1.0f / specialFireRate;
        //    Debug.Log("Cooldown is:" + fire2Cooldown);
        //    SpecialShoot();

        //    // Animation still scales on normal attackrate:
        //    SpecialAttackAnimation(1.0f / fireRate);
        //}
    }

    public virtual void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~layerIgnore))
        {
            //Debug.Log(hit.transform.name);
            Debug.DrawLine(fpsCam.transform.position, hit.point, Color.green, 2, false);

            // Check if hit object can be damaged:
            Hitable hitable = hit.transform.GetComponent<Hitable>();

            // Check if hit objects parent can be damaged:
            if (hitable == null) hitable = hit.transform.GetComponentInParent<Hitable>();

            if(hitable != null)
            {
                // Check if you are still hilding the weapon during calc:
                if(this.gameObject.activeSelf)
                    hitable.TakeDamge(damage);
            }

            if (hit.rigidbody != null)
            {
                // Check if you are still hilding the weapon during calc:
                if (this.gameObject.activeSelf)
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
            }

            // Check if you are still hilding the weapon during calc:
            if (this.gameObject.activeSelf) 
            { 
                // What particle:
                GameObject hitParticle;
                if (hit.transform.GetComponent<Enemy>() != null || hit.transform.GetComponentInParent<Enemy>() != null)
                {

                    hitParticle = Instantiate(enemyHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
                } 
                else
                {
                    hitParticle = Instantiate(enviormentHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
                }
            
                Destroy(hitParticle, 2f);
            }
        }
    }

    public virtual void AttackAnimation(float cooldown)
    {
        // Set shooting layer to max:
        anim.SetLayerWeight(1, 1);

        //Restart Animation:
        anim.Play("ResetFire1", 1);
        // float animspeed = fireRate;
        //anim.speed = animspeed;

        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, cooldown).OnComplete(() =>
        {
            // Set shooting layer to zero:
            // Not if player has swapped weapons:
            if(this.gameObject.activeSelf)
                anim.SetLayerWeight(1, 0);

        });

        
    }

    public virtual void SpecialShoot()
    {
        Debug.Log("Special Attack");
    }

    public virtual void SpecialAttackAnimation(float cooldown)
    {

        // Set shooting layer to max:
        anim.SetLayerWeight(3, 1);

        //Restart Animation:
        anim.Play("ResetFire2", 3);

        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, cooldown).OnComplete(() =>
        {
            // Set shooting layer to zero:
            // Not if player has swapped weapons:
            if (this.gameObject.activeSelf)
                anim.SetLayerWeight(3, 0);

        });
    }
    public void ResetEntryCooldown()
    {
        currentEntryCooldown = entryCooldown;
    }
}
