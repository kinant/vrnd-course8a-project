using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{
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
        ReleaseObject(e.controller.velocity, e.controller.angularVelocity);
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
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject(Vector3 velocity, Vector3 angularVelocity)
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
        }

        objectInHand = null;
    }
}
