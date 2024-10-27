using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ElevatorControls : MonoBehaviour
{
    public bool elevatorOn = false;
    [Header("Door Controls")]
    [SerializeField]
    private GameObject elevatorDoor;
    [SerializeField]
    private GameObject elevatorDoorCollider;

    [SerializeField]
    private Vector3 openPos;
    [SerializeField]
    private Vector3 closedPos;

    private float currentElevatorYPos;
    private float targetElevatorYPos;

    [Header("Lights")]
    private float lightSpeed = 100.0f;
    [SerializeField]
    private GameObject lights;
    private float lightsYPos;

    [Header("SceneSwap")]
    [SerializeField]
    private GameObject sceneSwapFade;
    private GameObject canvasRef;
    private GameObject fade;
    private Image fadeImage;
    private float currentAlpha = 0.0f;
    [SerializeField]
    private AudioFade crowdSource;

    private void Start()
    {
        currentElevatorYPos = openPos.y;
        targetElevatorYPos = currentElevatorYPos;
        elevatorDoorCollider.SetActive(false);
        lightsYPos = 2.0f;
        lights.transform.localPosition = new Vector3(0, lightsYPos, 0);
        canvasRef = GameManager.instance.UI;
    }
   

    private void Update()
    {
        if (elevatorOn)
        {
            // Elevator Door:
            currentElevatorYPos = Mathf.Lerp(currentElevatorYPos, targetElevatorYPos, Time.deltaTime * 10f);
            elevatorDoor.transform.localPosition = new Vector3(openPos.x, currentElevatorYPos, openPos.z);

            // Lights moving
            lightsYPos -= Time.deltaTime * lightSpeed;
            lights.transform.localPosition = new Vector3(0, lightsYPos, 0);
            if (lightsYPos < -8) lightsYPos = 27.0f;

            // ScreenFade:
            currentAlpha = currentAlpha + Time.deltaTime * 0.3f;
            fadeImage.color = new Color(1, 1, 1, currentAlpha);

            if(crowdSource != null && crowdSource.transitionInProgress != true)
                crowdSource.transitionInProgress = true;

            if (currentAlpha >= 1.0F)
            {
                if (TutorialManager.Instance != null) Destroy(TutorialManager.Instance.gameObject);
                SceneManager.LoadScene("Arena");
            }
        }

    }

    public void StartElevator()
    {
        targetElevatorYPos = closedPos.y;
        elevatorDoorCollider.SetActive(true);
        elevatorOn = true;
        fade = Instantiate(sceneSwapFade, canvasRef.transform);
        fadeImage = fade.GetComponentInChildren<Image>();
    }
}
