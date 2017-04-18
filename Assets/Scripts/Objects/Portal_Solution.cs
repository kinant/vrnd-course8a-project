using UnityEngine;

public class Portal_Solution : Portal {

    public Transform portalEntrance;
    public Transform portalExit;

    protected override void Awake()
    {
        m_transform = GetComponent<Transform>();

        // set parents
        portalEntrance.SetParent(m_transform);
        portalExit.SetParent(m_transform);

        portalIn = portalEntrance;
        portalOut = portalExit;

        // create the line
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = lineMaterial;

        lineRenderer.SetPosition(0, portalIn.position);
        lineRenderer.SetPosition(1, portalOut.position);
    }
}
