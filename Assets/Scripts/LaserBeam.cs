using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;

    public float speed = 2.0f;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = endPos - startPos;
        Vector3 normDir = dir.normalized;
        this.gameObject.transform.position += normDir * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerStats>())
        {
            other.gameObject.GetComponentInParent<PlayerStats>().PlayerDamage(damage);
            GameManager.instance.drawDamageArrow(startPos);
            Destroy(this.gameObject, 0.1f);
        }
        else
        {
           Destroy(this.gameObject, 0.1f);
        }
    }

}
