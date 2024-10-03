using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPosSingleton : MonoBehaviour
{
    public RectTransform canvasTransform;

    private void Awake()
    {
        canvasTransform = GetComponent<RectTransform>();
        Debug.Log("base transform " + $"{canvasTransform == null}");
    }
}
