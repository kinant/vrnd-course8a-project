using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Transform portalIn;
    public Transform portalOut;
    public Material lineMaterial;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.numPositions = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        // lineRenderer.startColor = Color.green;
        // lineRenderer.endColor = Color.green;
        // Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        // newMaterial.SetColor("_Color", Color.green);
        // lineRenderer.material = newMaterial;
        lineRenderer.material = lineMaterial;
    }

    // Update is called once per frame
    void Update () {
        if (lineRenderer == null) {
            return;
        }

        lineRenderer.SetPosition(0, portalIn.position);
        lineRenderer.SetPosition(1, portalOut.position);

        var distance = Vector3.Distance(portalIn.position, portalOut.position);
        lineRenderer.materials[0].mainTextureScale = new Vector3(distance, 1, 1);
    }
}
