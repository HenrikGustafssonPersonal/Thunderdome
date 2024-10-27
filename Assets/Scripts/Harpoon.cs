using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField]
    private GameObject ropeAttach;

    [SerializeField]
    private LineRenderer rope;

    [SerializeField]
    private GameObject trail;

    [SerializeField]
    private float pullForce = 50.0f;

    private GameObject shotGunTracking;
    // Start is called before the first frame update
    private void Start()
    {
        shotGunTracking = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShotGun>().muzzleFlash.gameObject;
    }

    private void Update()
    {
        rope.SetPosition(0, ropeAttach.transform.position);
        rope.SetPosition(1, shotGunTracking.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Enemy>() != null)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            rope.enabled = false;
            trail.GetComponent<TrailRenderer>().emitting = false;

            if (collision.gameObject.GetComponentInParent<FlyerEnemy>() != null)
                DestroyFlyer(collision.gameObject);
            else
                PullEnemy(collision.gameObject);
            Debug.Log("Hit ENEMY");
            
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            rope.enabled = false;
            trail.GetComponent<TrailRenderer>().emitting = false;

            Destroy(gameObject, 20f);
        }
    }

    private void PullEnemy(GameObject enemy)
    {
        float animSpeed = enemy.GetComponentInParent<Enemy>().gameObject.GetComponent<Animator>().speed;
        enemy.GetComponentInParent<Enemy>().gameObject.GetComponent<Animator>().speed = 0.0f;
        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, 0.3f).OnComplete(() =>
        {
            if (this.gameObject.activeSelf)
            {
                Vector3 force = GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position;
                enemy.GetComponentInParent<Enemy>().gameObject.GetComponent<Rigidbody>().AddForce(force * pullForce);
                enemy.GetComponentInParent<Enemy>().gameObject.GetComponent<Animator>().speed = animSpeed;
                Destroy(gameObject, 20f);
            }
        });

    }

    private void DestroyFlyer(GameObject flyer)
    {
        flyer.GetComponentInParent<Enemy>().TakeDamge(100f);
        Destroy(gameObject, 0.1f);
    }
}
