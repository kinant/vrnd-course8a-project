using UnityEngine;

// class for the tutorial triggers. 
// the first part of the tutorial has two triggers the player must reach in order to proceed
public class ExitTrigger : MonoBehaviour {

    // handle collisions 
    private void OnTriggerEnter(Collider other)
    {
        // we check that the player's head has entered the trigger
        if (other.gameObject.tag.Equals("PlayerHead")) {
            TutorialManager.Instance.SetState(TutorialManager.TutorialState.Complete);
        }
    }
}
