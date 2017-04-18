using UnityEngine;

// the ball used for the puzzle levels
public class Ball_Level : Ball {

    public LevelManager levelManager; // reference to the level manager

    // we override the reset ball to tell the level manager to reset the level (when ball hits ground)
    protected override void ResetBall() {
        base.ResetBall();

        levelManager.ResetLevel();
    }

    // handle what happens when ball enters other triggers
    void OnTriggerEnter(Collider other)
    {
        // if the ball is invalid, player cheated, so we do nothing
        if (isInvalid) {
            return;
        }

        string tag = other.gameObject.tag;

        // if the ball hits the goal, then we check with the level manager if we won
        if (tag.Equals("Goal"))
        {
            levelManager.CheckWin();
        }

        // if the ball hits a star, then we tell the level manager to collect it
        if(tag.Equals("Star"))
        {
            levelManager.CollectStar();

            // hide the star
            other.gameObject.SetActive(false);
        }
    }

    // we override so that we can play the appropiate sound when the ball leaves the play area
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject.tag.Equals("PlayArea") && isBeingHeld) {
            levelManager.PlayIncorrectSound();
        }
    }
}
