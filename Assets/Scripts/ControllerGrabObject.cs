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

        if(collidingObject.tag.Equals("Throwable") || collidingObject.tag.Equals("Structure") || collidingObject.tag.Equals("Funnel"))
        {
            GrabObject();
        }
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
        else if (objectInHand.tag.Equals("Structure") || objectInHand.tag.Equals("Funnel")) {
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
        // print("hit trigger: " + other.gameObject.tag + ", " + other.gameObject.name);
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        // print("stay trigger: " + other.gameObject.tag + ", " + other.gameObject.name);
        SetCollidingObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        // print("exit trigger: " + other.gameObject.tag + ", " + other.gameObject.name);
        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;

        if (objectInHand.tag.Equals("Funnel")) {
            objectInHand = objectInHand.transform.parent.gameObject;
        }

        // print("GRABBING OBJECT: " + objectInHand.name);

        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.isKinematic = true;
        }

        objectInHand.transform.SetParent(transform);

        // this is the ball...set being held flag
        if (objectInHand.GetComponent<Ball>())
        {
            objectInHand.GetComponent<Ball>().isBeingHeld = true;
        }

        collidingObject = null;

        if (!objectInHand.tag.Equals("Throwable"))
        {
            // toggle colliders off while holding an object
            ToggleColliders(objectInHand, false);
        }
    }

    private void ReleaseObject(Vector3 velocity, Vector3 angularVelocity, bool isKinematic)
    {
        // this is the ball...set being held flag
        if (objectInHand.GetComponent<Ball>())
        {
            objectInHand.GetComponent<Ball>().isBeingHeld = false;
        }

        // turn the colliders back on
        ToggleColliders(objectInHand, true);

        objectInHand.transform.SetParent(null);
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.isKinematic = isKinematic;
            rb.velocity = velocity * throwForce;
            rb.angularVelocity = angularVelocity;
        }
        objectInHand = null;
    }

    public static void ToggleColliders(GameObject obj, bool toggle)
    {

        if (obj.GetComponent<Collider>())
        {
            obj.GetComponent<Collider>().enabled = toggle;
        }

        // print("checking parent: " + obj.name);
        Transform portalTransform = obj.transform;
        int childCount = obj.transform.childCount;

        // no more children to look for colliders, return
        if (childCount == 0)
        {
            return;
        }

        // print("this parent has: " + childCount + " children.");

        for (int i = 0; i < childCount; i++)
        {
            GameObject go = portalTransform.GetChild(i).gameObject;
            // print("checking child: " + go.name);

            if (go.GetComponent<Collider>())
            {
                print("this child has a colider!");
                go.GetComponent<Collider>().enabled = toggle;
            }

            // toggle colliders in children
            if (go.transform.childCount > 0)
            {
                ToggleColliders(go, toggle);
            }
            go = null;
        }
    }
}
