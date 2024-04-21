using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererSettings : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    Vector3[] points;

    private void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        points = new Vector3[2];
        points[0] = Vector3.zero;
        points[1] = transform.position + new Vector3(0,0,20);
        _lineRenderer.SetPositions(points);
        _lineRenderer.enabled = true;
    }
}
