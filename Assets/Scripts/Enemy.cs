using UnityEngine.Animations.Rigging;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : Hitable
{
    public float movementSpeed = 10.0f;
    public float damage = 3.0f;
    //healingAmountOnDeath
    public int playerHealAmountOnDeath;
    public int totalScoreValue = 100;
    public float cheerScoreValue= 100;

    protected Animator anim;
    [SerializeField]
    private GameObject deathParticlesGameObjects;

    protected ParticleSystem[] deathParticles;
    public GameObject reflectionTarget;

    protected Rigidbody[] allRBs;



    void Awake()
    {

        deathParticles = deathParticlesGameObjects.GetComponentsInChildren<ParticleSystem>();


    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        allRBs = gameObject.GetComponentsInChildren<Rigidbody>();

        //Debug.Log(gameObject.name + allRBs.Length);
        foreach (Rigidbody rb in allRBs)
        {
            rb.isKinematic = true;
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Destroy()
    {
        anim.enabled = false;
        GameManager.instance.addToCheerScore(cheerScoreValue);
        GameManager.instance.addToTotalScore(totalScoreValue);


        foreach (Rigidbody rb in allRBs)
        {
            rb.isKinematic = false;
        }

        SetGameLayerRecursive(gameObject, 9);

        if (deathParticles != null)
        {
            foreach (ParticleSystem d in deathParticles)
            {
                d.Play();
            }
            
        }

        if(GetComponentInChildren<LineRenderer>() != null)
        {
            LineRenderer[] lrs = GetComponentsInChildren<LineRenderer>();
            foreach (LineRenderer lr in lrs)
            {
                Destroy(lr);
            }
        }

        DeathExpotion();
        if (GetComponent<NavMeshAgent>() != null)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<EnemyAI>().disabledAI = true;
        }

        GameManager.instance.playerHeal(playerHealAmountOnDeath);
        Destroy(gameObject, 5);
    }

    private void DeathExpotion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(reflectionTarget.transform.position, 5.0f);
        foreach (Collider hit in hitColliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(20.0f, reflectionTarget.transform.position, 5.0f);
        }
    }
    void SetAimTarget()
    {
        Debug.Log("TargetSet");
        GetComponent<EnemyAI>().StartAI();
        //var data = new WeightedTransformArray();
        //if (gameObject.GetComponentInChildren<MultiAimConstraint>() != null)
        //{
            //data = gameObject.GetComponentInChildren<MultiAimConstraint>().data.sourceObjects;
            //data.Clear();
            //data.SetTransform(1, GameObject.FindGameObjectWithTag("Player").transform);
            //data.Add(new WeightedTransform(, 1));
            //gameObject.GetComponentInChildren<MultiAimConstraint>().data.sourceObjects = data;

            //gameObject.GetComponentInChildren<RigBuilder>().Build();
            //WeightedTransform weightedTransform = new WeightedTransform { transform = GameObject.FindGameObjectWithTag("Player").transform, weight = 1f };
           // var a = gameObject.GetComponentInChildren<MultiAimConstraint>().data.sourceObjects;
           // a = new WeightedTransformArray { weightedTransform };
           // a.SetWeight(0, 1);
            //gameObject.GetComponentInChildren<MultiAimConstraint>().data.sourceObjects.SetWeight(0, 1);
            //gameObject.GetComponentInChildren<MultiAimConstraint>().data.sourceObjects = a;
            //gameObject.GetComponentInChildren<RigBuilder>().Build();
            //gameObject.GetComponentInChildren<Rig>().weight = 1;
        //}

        //foreach (Rigidbody rb in allRBs)
        //{
          // rb.isKinematic = false;
        //}

    }
    protected void SetGameLayerRecursive(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);

        }
    }
}
