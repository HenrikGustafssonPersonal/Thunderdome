using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING,
        PAUSED
    }
    [System.Serializable]

    public class Wave
    {
        public string name;
        public Transform meleeEnemy;
        public Transform chaserEnemy;
        public Transform flyerEnemy;
        public Transform rangerEnemy;

        public int meleeCount;
        public int chaserCount;
        public int flyerCount;
        public int rangerCount;

        [Header("Current Count")]

        public int currentMeleeCount = 0;
        public int currentChaserCount = 0;
        public int currentFlyerCount = 0;
        public int currentRangerCount = 0;

        public float rate;
    }

    public Wave[] waves;
    private int nextWave = -1;
    [SerializeField] GameObject VictoryScreen;

    public Transform[] spawnPoints;

    //On entire game complete
    private bool gameComplete = false;

    //Between rounds

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private float searchCountDown = 1f;//Interval time between checking if enemies alive in wave, taxing operation
    private SpawnState state = SpawnState.COUNTING;
    private bool isPaused = true;

    void Start()
    {

        waveCountDown = timeBetweenWaves;


    }
    private void Update()

    {


        if (isPaused)
        {
            state = SpawnState.PAUSED;
            return;
        }  

        if (state == SpawnState.WAITING)
        {
            //Check if  enemies are still alive
            if (EnemyIsAlive()  == false)
            {

                //Enter paused state until new wave is triggered            
                isPaused = true;
    

            } else if (gameComplete == true)
            {
                //Insert end credits

                return;

            }
            else
            {
                return;
            }
        } 
        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                //Start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }   


        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }
    void WaveCompleted()
    {
        Debug.Log("Wave completed!");


            waveCountDown = timeBetweenWaves;


            if (nextWave + 1 > waves.Length - 1)
            {
                Debug.Log("Completed all waves. GAME OVER");
            //GAME FINSIHED...
                GameManager.instance.enableWinState();
                gameComplete = true;


            return; 
            }
            nextWave++;

    }

    public void BeginNextRound()
    {
        if (isPaused)
        {
            isPaused = false;
            WaveCompleted();    
            

        }

    }
    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        

        }
        return true;
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);

        state = SpawnState.SPAWNING;
        for (int i = 0; i < _wave.meleeCount + _wave.chaserCount + _wave.rangerCount + _wave.flyerCount; i++)
        {
            List<int> availableEnemies = new List<int>();

            if (_wave.currentMeleeCount < _wave.meleeCount)
            {
                availableEnemies.Add(0); 
            }
            if (_wave.currentChaserCount < _wave.chaserCount)
            {
                availableEnemies.Add(1);
            }
            if (_wave.currentRangerCount < _wave.rangerCount)
            {
                availableEnemies.Add(2); 
            }
          
            if (_wave.currentFlyerCount < _wave.flyerCount)
            {
                availableEnemies.Add(3); 
            }

            if (availableEnemies.Count == 0)
            {
                break;
            }

            int selectedEnemyIndex = Random.Range(0, availableEnemies.Count);
            int selectedEnemy = availableEnemies[selectedEnemyIndex];

            switch (selectedEnemy)
            {
                case 0:
                    _wave.currentMeleeCount++;
                    SpawnEnemy(_wave.meleeEnemy);
                    break;
                case 1:
                    _wave.currentChaserCount++;
                    SpawnEnemy(_wave.chaserEnemy);
                    break;
                case 2:
                    // Spawn ranger enemy
                    _wave.currentRangerCount++;
                    SpawnEnemy(_wave.rangerEnemy);
                    break;
                case 3:
                    _wave.currentFlyerCount++;
                    SpawnEnemy(_wave.flyerEnemy);
                    break;
            }



            yield return new WaitForSeconds(1f / _wave.rate);//Wait for next spawn
        }
      
        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //Spawn enemy
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        Debug.Log("Spawning enemy" + _enemy.name);
    }
}
