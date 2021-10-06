using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EditorGraph : MonoBehaviour
{
    [SerializeField]
    private string graphName;

    [SerializeField]
    private AnimationCurve curve = null;

    private float currentX = 0.0f;
    private int currentKeyIndex = 0;

    public void Initialize(string graphName)
    {
        this.graphName = graphName;
        curve = new AnimationCurve();
    }

    public void AddDataPoint(double xIncrement, double yValue)
    {
        currentX += (float) xIncrement;;
        curve.AddKey(currentX, (float) yValue);
        curve.SmoothTangents(currentKeyIndex, 0.5f);
        currentKeyIndex++;
    }
}