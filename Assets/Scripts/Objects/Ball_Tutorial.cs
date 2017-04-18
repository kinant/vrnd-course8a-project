using UnityEngine;

// the ball used for the tutorial levels
public class Ball_Tutorial : Ball {

    // handle what happens when ball enters colliders
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        // if it hits the goal, all we do is reset it
        // if the ball is invalid, we also play the failure song (dont cheat!)
        if (tag.Equals("Goal"))
        {
            if (isInvalid)
            {
                // play incorrect sound...
                TutorialManager.Instance.PlayIncorrectSound();
                return;
            }

            ResetBall();

            // if the ball made a valid hit to the goal, then we want to go to the next state of the tutorial.
            // we check what state we are currently in, and then set the appropiate next state
            switch (TutorialManager.Instance.CurrentState) {
                case TutorialManager.TutorialState.Grabbing:
                    TutorialManager.Instance.SetState(TutorialManager.TutorialState.Spawning);
                    break;
                case TutorialManager.TutorialState.Spawning:
                    TutorialManager.Instance.SetState(TutorialManager.TutorialState.Spawn_WoodPlank);
                    break;
                case TutorialManager.TutorialState.Spawn_WoodPlank:
                    TutorialManager.Instance.SetState(TutorialManager.TutorialState.Spawn_Funnel);
                    break;
                case TutorialManager.TutorialState.Spawn_Funnel:
                    TutorialManager.Instance.SetState(TutorialManager.TutorialState.Spawn_Portal);
                    break;
                case TutorialManager.TutorialState.Spawn_Portal:
                    TutorialManager.Instance.SetState(TutorialManager.TutorialState.Complete);
                    break;
                default:
                    break;
            }
        }
    }

    // we overide so that we can play a sound when the user leaves the trigger area while holding the ball
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject.tag.Equals("PlayArea") && isBeingHeld)
        {
            TutorialManager.Instance.PlayIncorrectSound();
        }
    }

    // we override to play a sound when this ball hits the ground (remember that this ball is different than the puzzle ball)
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.tag.Equals("Ground"))
        {
            TutorialManager.Instance.PlayIncorrectSound();
        }
    }
}
