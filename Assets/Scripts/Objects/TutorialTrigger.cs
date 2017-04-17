using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("PlayerHead")) {

            if (TutorialManager.Instance.CurrentState == TutorialManager.TutorialState.Teleport)
            {
                TutorialManager.Instance.SetState(TutorialManager.TutorialState.Elevate);
                Destroy(gameObject);
            }
            else if (TutorialManager.Instance.CurrentState == TutorialManager.TutorialState.Elevate) {
                TutorialManager.Instance.SetState(TutorialManager.TutorialState.Grabbing);
                Destroy(gameObject);
            }
        }
    }
}
