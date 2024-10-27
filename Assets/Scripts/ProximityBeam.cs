using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityBeam : MonoBehaviour
{
    private LineRenderer lr;
    private GameObject playerRef;

    private void Start()
    {
        lr = GetComponentInChildren<LineRenderer>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(this.gameObject.transform.position, playerRef.transform.position);
        SetColor((distance - 20) / 100);
    }

    private void Update()
    {
        float distance = Vector3.Distance(this.gameObject.transform.position, playerRef.transform.position);
        Debug.Log(distance);

        if (distance < 20.0f)
            return;

        if (distance > 120.0f)
            return;

        SetColor((distance - 20) / 100);
    }

    private void SetColor(float proximity)
    {
        //lr.material.SetColor("_Color", new Color(1f, 0.75f, 0f, proximity));

        lr.startColor = new Color(1f, 0.75f, 0f, proximity);

    }
}
