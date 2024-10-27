using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    public GameObject reflectionTarget;
    private float range = 50f;
    public int splitAmount = 1;

    public int rayAttempts = 10;


    public GameObject enemyHitParticles;
    public GameObject reflectHitParticles;

    public GameObject reflectShotLine;

    public void Reflect(Vector3 hitlocation, List<GameObject> prevReflectObjs, float damage)
    {
        prevReflectObjs.Add(this.gameObject);

        // Get all enemies and reflectors in scene:
        List<GameObject> reflectionTargetsObjects = GetAllReflectionTargets(hitlocation);

        // Get all targets within radius:
        List<GameObject> validTargets = GetValidTargets(prevReflectObjs, reflectionTargetsObjects);

        // No valid targets
        if(validTargets.Count <= 0) return;

        int randomTarget = Random.Range(0, validTargets.Count);

        Vector3 aimLocation = GetAimLocation(validTargets[randomTarget]);
        //Debug.DrawLine(hitlocation, aimLocation, Color.yellow, 2, false);
        RenderGunShot(hitlocation, aimLocation);

        GameObject hitParticle;
        if (validTargets[randomTarget].GetComponent<Reflector>() != null)
        {
            hitParticle = Instantiate(reflectHitParticles, aimLocation, Quaternion.LookRotation((hitlocation-aimLocation)));
            Destroy(hitParticle, 2f);

            validTargets[randomTarget].GetComponent<Reflector>().Reflect(aimLocation, prevReflectObjs, damage);
        }
        else if (validTargets[randomTarget].GetComponentInParent<Enemy>() != null)
        {
            hitParticle = Instantiate(enemyHitParticles, aimLocation, Quaternion.LookRotation((hitlocation - aimLocation)));
            Destroy(hitParticle, 2f);

            validTargets[randomTarget].GetComponentInParent<Enemy>().TakeDamge(damage);

            prevReflectObjs = new List<GameObject>();
        }
    }

    private List<GameObject> GetAllReflectionTargets(Vector3 hitlocation)
    {
        List<GameObject> objs = new List<GameObject>(GameObject.FindGameObjectsWithTag("ReflectTarget"));
        List<GameObject> closeObjs = new List<GameObject>();
        
        
        foreach (GameObject obj in objs)
        {
            Vector3 newVec = obj.transform.position - hitlocation;


            if (newVec.magnitude < range){
                if (obj.transform.parent.GetComponentInChildren<Reflector>() != null)
                    closeObjs.Add(obj.transform.parent.GetComponentInChildren<Reflector>().gameObject);
                else if ((obj.transform.GetComponentInParent<Enemy>() != null))
                    closeObjs.Add(obj.transform.GetComponentInParent<Enemy>().gameObject);
            }
        }
        return closeObjs;
    }

    private List<GameObject> GetValidTargets(List<GameObject> prevObjs, List<GameObject> allTargets)
    {
        foreach (GameObject obj in prevObjs)
        {
            if (allTargets.Contains(obj))
                allTargets.Remove(obj);
        }
        return allTargets;
    }

    private Vector3 GetAimLocation(GameObject obj)
    {
        if (obj.GetComponentInChildren<Enemy>() != null)
            return obj.GetComponent<Enemy>().reflectionTarget.transform.position;
        else if (obj.GetComponent<Reflector>() != null)
            return obj.GetComponent<Reflector>().reflectionTarget.transform.position;
        else return new Vector3(0, 0, 0);
    }

    private bool hitValidTarget(RaycastHit hit, Vector3 start, Vector3 end)
    {
        if ((end - start).magnitude < (hit.point - start).magnitude)
            return false;
        if (hit.transform == null)
            return false;

        if (hit.transform.GetComponentInParent<Enemy>() != null)
            return true;
        else if (hit.transform.GetComponent<Reflector>() != null)
            return true;
        else return false;
    }

    private void RenderGunShot(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer lr = Instantiate(reflectShotLine).GetComponent<LineRenderer>();
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
    }
}
