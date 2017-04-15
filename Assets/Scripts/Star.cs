﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    public float rotationSpeed = 95f;

    private Transform m_transform;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("BALL HAS HIT STAR");
        // Destroy(gameObject);
        LevelManager.Instance.CollectStar();
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        m_transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        m_transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f));
	}
}
