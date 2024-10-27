using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
    public LineRenderer circleRenderer;
    public int resolution = 10;  

    void Start()
    {
    }



    public void DrawCircle(Vector3 currentPosition, float radius)
    {
        circleRenderer.loop = true; 
        circleRenderer.positionCount = resolution;

        float angle = 0f;

        for (int i = 0; i < resolution; i++)
        {
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            circleRenderer.SetPosition(i, new Vector3(currentPosition.x + x, 0f, currentPosition.z + z));

            angle += 2f * Mathf.PI / resolution;
        }
    }
}
