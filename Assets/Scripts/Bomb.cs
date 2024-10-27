using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hitable
{
    [SerializeField]
    float radius = 9f;
    [SerializeField]
    float damage = 50.0f;

    [SerializeField]
    float selfDestructTimer = 5.0f;

    [SerializeField]
    private ParticleSystem explosionParticles;

    private void Update()
    {
        TakeDamge(Time.deltaTime * 1/selfDestructTimer);
    }
    public override void Destroy()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        //explosionParticles.Play();
        ParticleSystem particles = Instantiate(explosionParticles, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(particles, 5);
        ExplosionDamage(gameObject.transform.position, radius);
        Destroy(this.gameObject, .2f);
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider hitCollider in hitColliders)
        {
            //hitCollider.SendMessage("AddDamage");
            GameObject hitGameObject = null;
            if (hitCollider.GetComponentInParent<Hitable>() != null)
            {
                hitGameObject = hitCollider.GetComponentInParent<Hitable>().gameObject;

            }
            else if (hitCollider.GetComponent<Hitable>() != null)
            {
                hitGameObject = hitCollider.GetComponent<Hitable>().gameObject;
            }
            else if (hitCollider.GetComponentInParent<PlayerStats>() != null)
            {
                hitCollider.GetComponentInParent<PlayerStats>().PlayerDamage(damage);

            }

            if (hitGameObject != null)
            {
                hitGameObject.GetComponent<Hitable>().TakeDamge(damage);
                if(hitCollider.GetComponent<Rigidbody>() != null)
                {
                    hitCollider.GetComponent<Rigidbody>().AddExplosionForce(50.0f, gameObject.transform.position, radius*2, 3.0f);
                }
            }
        }
    }
}
