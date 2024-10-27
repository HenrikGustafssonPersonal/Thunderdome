using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public enum SpawnType {Melee, Chaser, Ranged, Flyer }
    public SpawnType enemyToSpawn = SpawnType.Melee;

    [SerializeField]
    private Transform spawnTransform;

    public GameObject MeleeEnemyPrefab;
    public GameObject ChaserEnemyPrefab;
    public GameObject RangedEnemyPrefab;
    public GameObject FlyerEnemyPrefab;

    [SerializeField]
    private ParticleSystem landingParticles;
    public void LandingEvent()
    {
        landingParticles.Play();
        GameObject enemy;
        switch (enemyToSpawn)
        {
            case SpawnType.Melee:
                enemy = Instantiate(MeleeEnemyPrefab, spawnTransform.position, spawnTransform.rotation);
                break;
            case SpawnType.Chaser:
                enemy = Instantiate(ChaserEnemyPrefab, spawnTransform.position, spawnTransform.rotation);
                break;
            case SpawnType.Ranged:
                enemy = Instantiate(RangedEnemyPrefab, spawnTransform.position, spawnTransform.rotation);
                break;
            case SpawnType.Flyer:
                enemy = Instantiate(FlyerEnemyPrefab, spawnTransform.position, spawnTransform.rotation);
                break;
            default:
                enemy = Instantiate(MeleeEnemyPrefab, spawnTransform.position, spawnTransform.rotation);
                break;
        }
        enemy.transform.SetParent(GameObject.FindGameObjectWithTag("AllEnemies").transform);

        Destroy(gameObject, 2f);
    }
}
