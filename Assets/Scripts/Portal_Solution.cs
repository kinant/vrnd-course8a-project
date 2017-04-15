using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Solution : MonoBehaviour {

    public Transform portalEntrance;
    public Transform portalExit;
    public Material lineMaterial;
    public Transform ball;
    public float portalExitForce = 2.0f;

    private Transform portalIn;
    private Transform portalOut;

    private Transform m_transform;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();

        // set parents
        portalEntrance.SetParent(m_transform);
        portalExit.SetParent(m_transform);

        ball = GameObject.Find("Ball").transform;

        portalIn = portalEntrance;
        portalOut = portalExit;

        // create the line
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.numPositions = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = lineMaterial;

        lineRenderer.SetPosition(0, portalIn.position);
        lineRenderer.SetPosition(1, portalOut.position);


    }

    private void Start()
    {
        // m_transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        // Find the ball

    }


    public void BallEnteredPortal() {
        ball.position = portalOut.transform.position;

        // we set the velocity:
        Rigidbody rb = ball.gameObject.GetComponent<Rigidbody>();
        if (rb) {
            // rb.velocity = new Vector3(0f, 0f, rb.velocity.z); 
            rb.velocity = portalOut.transform.forward * portalExitForce;
        }
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
