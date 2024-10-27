using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private Vector3 spinDirection;

    public bool shouldSpin = true;

    // Update is called once per frame
    void Update()
    {
        if(shouldSpin)
            gameObject.transform.Rotate(spinDirection * speed * Time.deltaTime);
    }
}
