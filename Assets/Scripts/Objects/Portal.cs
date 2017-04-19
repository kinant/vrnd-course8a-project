using UnityEngine;

// the basic portal, this portal is a puzzle piece than can be used to teleport the ball from one
// location to another
public class Portal : MonoBehaviour {

    public GameObject portalEntrancePrefab; // the prefab for the portal entrance
    public GameObject portalExitPrefab; // the prefab for the portal exit
    public Material lineMaterial; // a material for the line renderer that shows between both portals
    public Transform ball; // we will cache the balls transform
    public float portalExitForce = 2.0f; // the force that will be applied to the balls velocity when it exits the portal

    protected Transform portalIn; // reference of the portal entrance transform
    protected Transform portalOut; // reference of the portal exit transform

    protected Transform m_transform; // reference to the main portal transform
    protected LineRenderer lineRenderer; // reference to the portals line renderer

    // we use awake to initialize the portal (doing it in start does NOT work)
    protected virtual void Awake()
    {
        // We create the child game objects, which are the two portal gates
        // We do this in code because it fixes a lot of bugs, and it allows
        // the user to change the model used for the portal gates.

        m_transform = GetComponent<Transform>();
        m_transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        // create the portals, and parent them to this game object
        GameObject entrance = Instantiate(portalEntrancePrefab, transform);
        entrance.transform.localPosition = new Vector3(0f, 0f, 0.4f);
        entrance.transform.localRotation = Quaternion.identity;

        GameObject exit = Instantiate(portalExitPrefab, transform);
        exit.transform.localPosition = new Vector3(0f, 0f, -0.4f);
        exit.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        // cache the transforms
        portalIn = entrance.transform;
        portalOut = exit.transform;

        // create the line
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.numPositions = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = lineMaterial;

        // set the line positions
        lineRenderer.SetPosition(0, portalIn.position);
        lineRenderer.SetPosition(1, portalOut.position);
    }

    private void Start()
    {
        // Find the ball
        ball = GameObject.FindWithTag("Throwable").transform;
    }

    // called by the child portal entrance when a ball enters its trigger collider
    public void BallEnteredPortal() {

        // we move the ball to the position of the exit
        ball.position = portalOut.transform.position;

        // we set the velocity:
        Rigidbody rb = ball.gameObject.GetComponent<Rigidbody>();
        if (rb) {
            rb.velocity = portalOut.transform.forward * portalExitForce;
        }
    }

    // Update is called once per frame
    void Update () {
        // we use update to update the line renderer (so that it changes when the portal gates are moved)
        if (lineRenderer == null) {
            return;
        }

        // set the new positions
        lineRenderer.SetPosition(0, portalIn.position);
        lineRenderer.SetPosition(1, portalOut.position);

        // this code is used to properly display a dotted line (out material uses a dotted line as a texture)
        // it is a nice effect
        var distance = Vector3.Distance(portalIn.position, portalOut.position);
        lineRenderer.materials[0].mainTextureScale = new Vector3(distance, 1, 1);
    }
}
