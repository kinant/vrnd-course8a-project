using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Vector3 startPosition;
    private Transform m_transform;
    private Rigidbody m_rb;

    private MeshRenderer m_renderer;
    private Color originalColor;

    public bool isBeingHeld = false;

    private bool isInvalid = false;

	// Use this for initialization
	void Start () {
        m_transform = GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody>();
        startPosition = m_transform.position;
        m_renderer = GetComponent<MeshRenderer>();
        originalColor = m_renderer.material.color;
	}

    void ResetBall() {
        m_transform.position = startPosition;

        m_renderer.material.color = originalColor;

        if (m_rb != null) {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }

        LevelManager.Instance.ResetLevel();

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
            LevelManager.Instance.CheckWin();
        }
        if(tag.Equals("Star"))
        {
            LevelManager.Instance.CollectStar();
            other.gameObject.SetActive(false);
            // Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("ball trigger exit: " + other.gameObject.tag);
        print("ball trigger exit: " + other.gameObject.name);

        if (other.gameObject.tag.Equals("PlayArea")) {

            print("player has left the play area! held: " + isBeingHeld);

            if (isBeingHeld)
            {
                print("player has left playing area holding the ball!");

                // Player has exited the play area with the ball, invalidate the ball
                m_renderer.material.color = Color.black;
                isInvalid = true;

                // play sound
                LevelManager.Instance.PlayIncorrectSound();
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
