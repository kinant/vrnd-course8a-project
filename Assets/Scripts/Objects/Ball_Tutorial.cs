using UnityEngine;

public class Ball_Tutorial : Ball {

    protected override void ResetBall()
    {
        base.ResetBall();
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag.Equals("Goal"))
        {
            if (isInvalid)
            {
                // play incorrect sound...
                TutorialManager.Instance.PlayIncorrectSound();
                return;
            }

            ResetBall();

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

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject.tag.Equals("PlayArea") && isBeingHeld)
        {
            TutorialManager.Instance.PlayIncorrectSound();
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.tag.Equals("Ground"))
        {
            TutorialManager.Instance.PlayIncorrectSound();
        }
    }
}
