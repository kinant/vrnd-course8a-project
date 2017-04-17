﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Tutorial : Ball {

    protected override void OnTriggerEnter(Collider other)
    {

        if (isInvalid) {
            return;
        }

        string tag = other.gameObject.tag;

        if (tag.Equals("Goal"))
        {
            print("Ball has hit goal!");

            print("Current state is: " + TutorialManager.Instance.CurrentState);

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

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag.Equals("Ground"))
        {
            LevelManager.Instance.PlayIncorrectSound();
        }
    }
}
