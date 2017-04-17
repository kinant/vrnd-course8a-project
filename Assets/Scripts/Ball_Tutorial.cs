﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Tutorial : MonoBehaviour {

    private Vector3 startPosition;
    private Transform m_transform;
    private Rigidbody m_rb;

    private MeshRenderer renderer;
    private Color originalColor;

    public bool isBeingHeld = false;

    private bool isInvalid = false;

	// Use this for initialization
	void Start () {
        m_transform = GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody>();
        startPosition = m_transform.position;
        renderer = GetComponent<MeshRenderer>();
        originalColor = renderer.material.color;
	}

    void ResetBall() {
        m_transform.position = startPosition;

        renderer.material.color = originalColor;

        if (m_rb != null) {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }

        // LevelManager.Instance.ResetLevel();

        isInvalid = false;
    }

    private void OnTriggerEnter(Collider other)
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
                    GetComponent<SteamVR_LoadLevel>().Trigger();
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("PlayArea")) {
            if (isBeingHeld)
            {
                // Player has exited the play area with the ball, invalidate the ball
                renderer.material.color = Color.black;
                isInvalid = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground")) {
            ResetBall();
        }
        
    }
}