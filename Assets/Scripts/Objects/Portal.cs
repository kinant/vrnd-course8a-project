using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject portalEntrancePrefab;
    public GameObject portalExitPrefab;
    public Material lineMaterial;
    public Transform ball;
    public float portalExitForce = 2.0f;

    private Transform portalIn;
    private Transform portalOut;

    private Transform m_transform;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        // We create the child game objects, which are the two portal gates
        // We do this in code because it fixes a lot of bugs, and it allows
        // the user to change the model used for the portal gates.

        m_transform = GetComponent<Transform>();
        m_transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        // create the portals
        GameObject entrance = Instantiate(portalEntrancePrefab, transform);
        // entrance.transform.SetParent(transform);
        entrance.transform.localPosition = new Vector3(0f, 0f, 0.4f);
        entrance.transform.localRotation = Quaternion.identity;

        GameObject exit = Instantiate(portalExitPrefab, transform);
        // exit.transform.SetParent(transform);
        exit.transform.localPosition = new Vector3(0f, 0f, -0.4f);
        exit.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        portalIn = entrance.transform;
        portalOut = exit.transform;
    }

    private void Start()
    {
        // Find the ball
        ball = GameObject.Find("Ball").transform;
        
        // create the line
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = lineMaterial;

        lineRenderer.SetPosition(0, portalIn.position);
        lineRenderer.SetPosition(1, portalOut.position);
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
