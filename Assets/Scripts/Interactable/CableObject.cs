using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode()]

public class CableObject : MonoBehaviour
{

    public bool colorRed;
    public GameObject mesh;

    float timer;
    float speed;
    Color colorStart;

    public Transform[] points;
    public LineRenderer lineRender;
    Color c1 = Color.white;
    Color c2 = Color.red;

    private void Start()
    {
        lineRender.positionCount = points.Length;
    }

    void Update()
    {
        ChangeColor();
        LineRender();
    }

    void LineRender()
    {
        for (int i = 0; i < points.Length; i++)
        {
            lineRender.SetPosition(i, points[i].position);
        }
    }

    void ChangeColor()
    {
        timer = Mathf.Clamp01(timer);
        if (colorRed)
        {
            //timer += Time.deltaTime * speed;
            //mesh.gameObject.GetComponent<LineRenderer>().material.color = Color.Lerp(colorStart, Color.red, timer);
        }
        else
        {
            //timer -= Time.deltaTime * speed;
            //mesh.gameObject.GetComponent<LineRenderer>().material.color = Color.Lerp(colorStart, Color.red, timer);
        }
    }

}
