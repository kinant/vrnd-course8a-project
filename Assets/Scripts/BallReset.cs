﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

    private Vector3 startPosition;
    private Transform m_transform;
    private Rigidbody m_rb;

	// Use this for initialization
	void Start () {
        m_transform = GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody>();
        startPosition = m_transform.position;
	}

    void ResetBall() {
        m_transform.position = startPosition;

        if (m_rb != null) {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground")) {
            ResetBall();
        }
    }
}
