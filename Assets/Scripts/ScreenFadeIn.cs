using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeIn : MonoBehaviour
{

    [SerializeField]
    private GameObject sceneSwapFade;
    private GameObject canvasRef;
    private GameObject fade;
    private Image fadeImage;
    private float currentAlpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        canvasRef = GameManager.instance.UI;
        fade = Instantiate(sceneSwapFade, canvasRef.transform);
        fadeImage = fade.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeImage != null)
        {
            // ScreenFade:
            currentAlpha = currentAlpha - Time.deltaTime * 0.3f;
            Color setColor = fadeImage.color;
            setColor.a = currentAlpha;
            fadeImage.color = setColor;
        }


        if (currentAlpha <= 0.0F)
        {
            Destroy(fade);
            Destroy(this.gameObject, 1.0f);
        }
    }
}
