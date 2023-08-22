using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]

public class CameraReferenceLocator : MonoBehaviour
{
    private Canvas canvas;
    private Camera _camera;
    private void Awake()
    {
        _camera = FindFirstObjectByType<Camera>();
        canvas = GetComponent<Canvas>();

        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = _camera;
    }
}
