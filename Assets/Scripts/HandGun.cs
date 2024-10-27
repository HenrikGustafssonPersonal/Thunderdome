using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HandGun : BaseGun
{
    public ParticleSystem muzzleFlash;
    public GameObject reflectHitParticles;

    public GameObject gunShotLine;
    [SerializeField]
    private Transform gunLinePos;

    [SerializeField]
    private GameObject bomb;

    private bool isThrowing = false;

    private void OnEnable()
    {
        isThrowing = false;
    }

    public override void Update()
    {
        if (currentEntryCooldown >= 0)
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

        //Special attack:
        //fire2Cooldown -= Time.deltaTime;
        float grenadeCd = GetComponentInParent<PlayerStats>().currentGrenadeCooldown;
        if (Input.GetButton("Fire2") && grenadeCd <= 0 && !isThrowing)
        {
            //fire2Cooldown = 1.0f / specialFireRate;
            Debug.Log("Cooldown is:" + grenadeCd);
            SpecialShoot();

            // Animation still scales on normal attackrate:
            SpecialAttackAnimation(1.0f / fireRate);
        }
    }

    public override void Shoot()
    {
        muzzleFlash.Play();
        AudioManager.Instance.PlaySFX("GunShot1");

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~layerIgnore))
        {
            //Debug.Log(hit.transform.name);
            Debug.DrawLine(fpsCam.transform.position, hit.point, Color.green, 2, false);
            RenderGunShot(gunLinePos.position, hit.point);

            // Check if hit object can be damaged:
            Hitable hitable = hit.transform.GetComponent<Hitable>();
            Reflector reflector = hit.transform.GetComponent<Reflector>();

            // Check if hit objects parent can be damaged:
            if (hitable == null) hitable = hit.transform.GetComponentInParent<Hitable>();

            if (hitable != null)
            {
                hitable.TakeDamge(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }

            if (reflector != null)
            {
                List<GameObject> gms = new List<GameObject>();
                reflector.Reflect(hit.point, gms, damage);
            }


            // What particle:
            GameObject hitParticle;
            if (hit.transform.GetComponent<Enemy>() != null || hit.transform.GetComponentInParent<Enemy>() != null)
            {
                hitParticle = Instantiate(enemyHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.GetComponent<Reflector>() != null || hit.transform.GetComponentInParent<Reflector>() != null)
            {
                hitParticle = Instantiate(reflectHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                hitParticle = Instantiate(enviormentHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
            }

            Destroy(hitParticle, 2f);
        }
    }

    public override void SpecialShoot()
    {
        Debug.Log("Bomb Attack");
        isThrowing = true;

        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, 0.35f).OnComplete(() =>
        {
            if (this.gameObject.activeSelf)
            {
                GameObject thrownBomb = Instantiate(bomb, fpsCam.transform.position + fpsCam.transform.forward, fpsCam.transform.rotation);
                thrownBomb.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * 10000f);
                GetComponentInParent<PlayerStats>().currentGrenadeCooldown = GetComponentInParent<PlayerStats>().grenadeCooldown;
                isThrowing = false;
            }

        });

    }

    public override void SpecialAttackAnimation(float cooldown)
    {
        cooldown += 1.28f;

        // Set greneeThrow layer to max:
        anim.SetLayerWeight(4, 1);

        //Restart Animation:
        anim.Play("ResetFire2", 4);

        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, cooldown).OnComplete(() =>
        {
            // Set  layer to zero:
            // Not if player has swapped weapons:
            if (this.gameObject.activeSelf)
                anim.SetLayerWeight(4, 0);

        });
    }


    void RenderGunShot(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer lr = Instantiate(gunShotLine).GetComponent<LineRenderer>();
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
    }
}
