using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{

    public float throwForce = 1.5f;

    private GameObject collidingObject;
    private GameObject objectInHand;
    private ControllerInputManager m_input_manager;

    private void Awake()
    {
        m_input_manager = GetComponent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        m_input_manager.TriggerUp += new InputEventHandler(DidTriggerPressUp);
        m_input_manager.TriggerDown += new InputEventHandler(DidTriggerPressDown);
    }

    private void OnDisable()
    {
        m_input_manager.TriggerUp -= new InputEventHandler(DidTriggerPressUp);
        m_input_manager.TriggerDown -= new InputEventHandler(DidTriggerPressDown);
    }

    private void DidTriggerPressDown(InputEventArgs e) {
        if (!collidingObject || objectInHand)
        {
            return;
        }

        GrabObject();
    }

    private void DidTriggerPressUp(InputEventArgs e) {
        if (!objectInHand)
        {
            return;
        }

        // check if the object is throwable or not...
        if (objectInHand.tag.Equals("Throwable"))
        {
            ReleaseObject(e.controller.velocity, e.controller.angularVelocity, false);
        }
        else if (objectInHand.tag.Equals("Structure")) {
            ReleaseObject(Vector3.zero, Vector3.zero, true);
        }
    }

    private void SetCollidingObject(Collider col)
    {

        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.isKinematic = true;
        }

        objectInHand.transform.SetParent(transform);
        collidingObject = null;
    }

    private void ReleaseObject(Vector3 velocity, Vector3 angularVelocity, bool isKinematic)
    {
        objectInHand.transform.SetParent(null);
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.isKinematic = isKinematic;
            rb.velocity = velocity * throwForce;
            rb.angularVelocity = angularVelocity;
        }
        objectInHand = null;
    }
}
