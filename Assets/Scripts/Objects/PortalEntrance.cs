using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a portal entrance is a child of the portal. For portals, the entrance portal is the one the ball has to collide with,
// in order to teleport to another location. This child is the one that has the collider attached, not its Portal parent.
public class PortalEntrance : MonoBehaviour {

    private Portal parentScript; // a reference to the Portal parent script

    private void Awake()
    {
        // just in case we do not assign it in the inspector, we look for the parent portal script
        if (parentScript == null)
        {
            parentScript = GetComponentInParent<Portal>();
        }
    }

    // handle collision with the ball (ball enters portal)
    private void OnTriggerEnter(Collider other)
    {
        // we want to check if the ball hit the portal entrance
        if (other.gameObject.GetComponent<Ball>())
        {
            // check if ball is being held (we do nothing if the user is holding it)
            if (other.gameObject.GetComponent<Ball>().isBeingHeld) {
                return;
            }

            // if not held, then we can teleport the ball
            // tell parent script to do so
            parentScript.BallEnteredPortal();
        }
    }
}
