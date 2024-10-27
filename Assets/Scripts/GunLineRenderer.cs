using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLineRenderer : MonoBehaviour
{
    public float lifeTime = 0.2f;
    private float currentLifeTime;

    private LineRenderer lr;


    private Vector3 lineStartPos;
    private Vector3 lineEndPos;
    private Vector3 lineVectorNorm;
    private float lineVectorMag;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        currentLifeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= lifeTime)
            Destroy(gameObject);
        else
        {
            float percent = currentLifeTime / lifeTime;

            ChangeAlpha(1-percent);
        }
    }
    private float previousAlpha = 1f;
    public void ChangeAlpha(float alpha)
    {
        if (Mathf.Abs(previousAlpha - alpha) > 0.01f)
        {
            previousAlpha = alpha;

            Gradient copyGradient = lr.colorGradient;
            GradientAlphaKey[] alphaArray = copyGradient.alphaKeys;
            alphaArray[1].alpha = alpha;
            copyGradient.alphaKeys = alphaArray;

            lr.colorGradient = copyGradient;
        }
    }
}
