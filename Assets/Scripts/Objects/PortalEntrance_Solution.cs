using UnityEngine;

public class PortalEntrance_Solution : MonoBehaviour {

    public Portal_Solution parentScript;

    private void Awake()
    {
        if (parentScript == null)
        {
            parentScript = GetComponentInParent<Portal_Solution>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
