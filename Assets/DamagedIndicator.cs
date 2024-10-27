using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamagedIndicator : MonoBehaviour
{
    [SerializeField]
    private Image damageIndicatorImage;
    public float startOpacity = 1.0f;
    public float decreaseRate = 1.0f;

    private Color tempColor;
    void Start()
    {
        tempColor = damageIndicatorImage.color;
        tempColor.a = startOpacity;
        
    }

    // Update is called once per frame
    void Update()
    {

        damageIndicatorImage.color = tempColor;
        if (tempColor.a <= 0)
        {
            Destroy(this.gameObject);
        } else
        {
            tempColor.a -= Time.deltaTime * decreaseRate;
        }
    }
}
