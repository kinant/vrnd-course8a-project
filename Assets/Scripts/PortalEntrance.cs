using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEntrance : MonoBehaviour {

    private Portal parentScript;

    private void Awake()
    {
        if (parentScript == null)
        {
            parentScript = GetComponentInParent<Portal>();
        }
    }

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ball has entered!");
        // this is the ball...set being held flag
        if (other.gameObject.GetComponent<Ball>())
        {
            // check if ball is being held
            if (other.gameObject.GetComponent<Ball>().isBeingHeld) {
                return;
            }

            // if not held, then we can teleport the ball
            parentScript.BallEnteredPortal();
        }
    }
}
