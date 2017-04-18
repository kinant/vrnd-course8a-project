using UnityEngine;

// this portal is the same as the main portal, only that it already has the gates as children. 
// this portal is used for solutions and for placing portals on the scene manually. We need this
// script because the main portal only instantiates the portal gates when we are in play mode. 
// We want a portal that can be manipulated in the scene editor.
public class Portal_Solution : Portal {

    // this portal already has the entrance and exit as children, so we get references to their transforms
    public Transform portalEntrance;
    public Transform portalExit;

    protected override void Awake()
    {
        m_transform = GetComponent<Transform>();

        // set parents for the child gates
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

        // set the line positions
        lineRenderer.SetPosition(0, portalIn.position);
        lineRenderer.SetPosition(1, portalOut.position);
    }
}
