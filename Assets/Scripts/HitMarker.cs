using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{

    private float targetOpacity = 0;
    private float opacity = 1.0f;

    private Image hitMarker;
    private Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        hitMarker = GetComponent<Image>();
        startColor = hitMarker.color;
    }

    // Update is called once per frame
    void Update()
    {
        opacity = Mathf.Lerp(opacity, targetOpacity, Time.deltaTime * 10.0f);
        Color c = new Color(startColor.r, startColor.g, startColor.b, opacity);
        hitMarker.color = c;
    }

    public void DoHitMarker()
    {
        opacity = 1.0f;
    }
}
