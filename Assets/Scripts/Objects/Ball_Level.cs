using UnityEngine;

public class Ball_Level : Ball {

    public LevelManager levelManager;

    protected override void ResetBall() {
        base.ResetBall();

        levelManager.ResetLevel();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isInvalid) {
            return;
        }

        string tag = other.gameObject.tag;

        if (tag.Equals("Goal"))
        {
            levelManager.CheckWin();
        }
        if(tag.Equals("Star"))
        {
            levelManager.CollectStar();
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject.tag.Equals("PlayArea")) {
            levelManager.PlayIncorrectSound();
        }
    }
}
