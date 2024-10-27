using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class FlyerEnemy : Enemy
{
    [SerializeField]
    float desiredHeight = 5.5f;

    [SerializeField]
    float ascendSpeed = 5.0f;

    [SerializeField]
    GameObject meshToMove;

    protected NavMeshAgent agent;
    protected Transform playerRef;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponent<Animator>();
        allRBs = gameObject.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in allRBs)
        {
            rb.isKinematic = true;
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update()
    {
        if (meshToMove.transform.localPosition.y < desiredHeight)
            meshToMove.transform.position = new Vector3(meshToMove.transform.localPosition.x, meshToMove.transform.localPosition.y + ascendSpeed * Time.deltaTime, meshToMove.transform.localPosition.z);

        if(meshToMove.transform.localPosition.y >= desiredHeight && currentHealth > 0.0f)
        {
            agent.enabled = true;
            GetComponent<FlyerAI>().disabledAI = false;
        }

    }
}
