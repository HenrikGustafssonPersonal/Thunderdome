using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : BaseGun
{
    [SerializeField]
    private int numberOfShots = 8;

    [SerializeField]
    private float bulletSpread = 0.08f;

    public ParticleSystem muzzleFlash;
    public GameObject reflectHitParticles;

    public GameObject gunShotLine;
    [SerializeField]
    private Transform gunLinePos;

    [SerializeField]
    private GameObject harpoon;

    public GameObject shotHarpoon;

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
        float hookCd = GetComponentInParent<PlayerStats>().currentHookCooldown;
        if (Input.GetButton("Fire2") && hookCd <= 0)
        {
            //fire2Cooldown = 1.0f / specialFireRate;
            Debug.Log("Cooldown is:" + hookCd);
            SpecialShoot();

            // Animation still scales on normal attackrate:
            SpecialAttackAnimation(1.0f / fireRate);
        }
    }
    public override void SpecialShoot()
    {
        Debug.Log("Harpoon Attack");

        if (this.gameObject.activeSelf)
        {
            Quaternion harpoonRot = new Quaternion(fpsCam.transform.rotation.x, fpsCam.transform.rotation.y, fpsCam.transform.rotation.z + 80, fpsCam.transform.rotation.w + 110);
            shotHarpoon = Instantiate(harpoon, fpsCam.transform.position + fpsCam.transform.forward*6, harpoonRot);
            shotHarpoon.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * 4000f);

            GetComponentInParent<PlayerStats>().currentHookCooldown = GetComponentInParent<PlayerStats>().hookCooldown;
        }

    }

    public override void Shoot()
    {
        FireOneBullet(new Vector3(0, 0, 0));

        Vector3 temp = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = 0; i < numberOfShots; i++)
        {
            float fi = 1.01f * i;
            float currentAngleSpread = (2.0f * Mathf.PI) * (fi / numberOfShots);

            temp =  new Vector3(Mathf.Cos(currentAngleSpread) * bulletSpread, Mathf.Sin(currentAngleSpread) * bulletSpread, Mathf.Cos(currentAngleSpread) * bulletSpread);

            //Vector3 moveX = new Vector3()
            temp = temp.x * transform.right + temp.z * transform.forward + temp.y* transform.up;
            //temp.x *= transform.right;
            //temp.y *= transform.forward.y;
            //temp.z *= transform.forward.x;
            FireOneBullet(temp);
        }

    }

    void FireOneBullet(Vector3 offset)
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward+offset, out hit, range, ~layerIgnore))
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
    void RenderGunShot(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer lr = Instantiate(gunShotLine).GetComponent<LineRenderer>();
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
    }
}
