using UnityEngine;

// class for the tutorial triggers. 
// the first part of the tutorial has two triggers the player must reach in order to proceed
public class TutorialTrigger : MonoBehaviour {

    // handle collisions 
    private void OnTriggerEnter(Collider other)
    {
        // we check that the player's head has entered the trigger
        if (other.gameObject.tag.Equals("PlayerHead")) {

            // we check what state we are in, for each trigger
            if (TutorialManager.Instance.CurrentState == TutorialManager.TutorialState.Teleport)
            {
                // set the next state
                TutorialManager.Instance.SetState(TutorialManager.TutorialState.Elevate);
                // destroy the trigger
                Destroy(gameObject);
            }
            // we are in the next state
            else if (TutorialManager.Instance.CurrentState == TutorialManager.TutorialState.Elevate) {
                TutorialManager.Instance.SetState(TutorialManager.TutorialState.Grabbing);
                Destroy(gameObject);
            }
        }
    }
}
