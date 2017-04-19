using UnityEngine;

// This class handles the grabbing of objects by the controllers
public class ControllerGrabObject : MonoBehaviour
{
    public float throwForce = 1.5f; // the force the object is thrown with after being released

    private GameObject collidingObject; // a reference to any grab-able objects the controllers collide with
    private GameObject objectInHand; // the currently held object by the controller
    private ControllerInputManager m_input_manager; // a reference to the controller input manager

    private void Awake()
    {
        // get the input manager component
        m_input_manager = GetComponentInParent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        // Subscribe to the events from the input manager
        m_input_manager.TriggerPressed += new InputEventHandler(DidTriggerPressDown);
        m_input_manager.TriggerUnpressed += new InputEventHandler(DidTriggerPressUp);
    }

    private void OnDisable()
    {
        // Unsubscribe to the events
        m_input_manager.TriggerPressed -= new InputEventHandler(DidTriggerPressDown);
        m_input_manager.TriggerUnpressed -= new InputEventHandler(DidTriggerPressUp);
    }

    // This function handles the event fired by the controller input manager
    // for when the trigger is pressed. The trigger is pressed to grab objects.
    private void DidTriggerPressDown(InputEventArgs e) {

        // if we have no colliding objects or if we 
        // already have an object being held, then we do nothing
        if (!collidingObject || objectInHand)
        {
            return;
        }

        // check that we can only grab the ball, the structures and the funnel or trampoline (special cases)
        if(collidingObject.tag.Equals("Throwable") || collidingObject.tag.Equals("Structure") || collidingObject.tag.Equals("Funnel") || collidingObject.tag.Equals("Trampoline"))
        {
            // proceed to grab the object
            GrabObject();
        }
    }

    // This function handles the event fired by the controller input manager
    // for when the trigger is released. This releases the currently held object (if any).
    private void DidTriggerPressUp(InputEventArgs e) {

        // if we are not holding an object, return
        if (!objectInHand)
        {
            return;
        }

        // check if the object is throwable or not...
        if (objectInHand.tag.Equals("Throwable"))
        {
            // Release the object and apply velocities
            ReleaseObject(e.controller.velocity, e.controller.angularVelocity, false);
        }
        else if (objectInHand.tag.Equals("Structure") || objectInHand.tag.Equals("Funnel") || objectInHand.tag.Equals("Trampoline")) {

            // Release the object but do not add any velocities to it
            ReleaseObject(Vector3.zero, Vector3.zero, true);
        }
    }

    // This function is used to set the collidingObject, for when the controller collides with
    // an object in the scene
    private void SetCollidingObject(Collider col)
    {
        // if we are already colliding with this object, or if this object
        // does not have a rigid body, then we return
        if (col.gameObject == collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        // set the colliding object
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        // set the colliding object
        SetCollidingObject(other);
    }


    public void OnTriggerStay(Collider other)
    {
        // set the colliding object
        SetCollidingObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        // controller no longer on this object, so we null the colliding object
        collidingObject = null;
    }

    // this function is used to grab (attach to controller) an object
    private void GrabObject()
    {

        // once we know we can grab an object, it is assigned as the object in hand
        objectInHand = collidingObject;

        // if it is tagged as a funnel or trampoline, we have a special case
        // these have the collider inn a child, so we want to attach the parent
        if (objectInHand.tag.Equals("Funnel") || objectInHand.tag.Equals("Trampoline")) {
            objectInHand = objectInHand.transform.parent.gameObject;
        }

        // cache the objects rigidbody
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

        // if the rigidbody is found, we set the isKinematic property to true
        if (rb != null) {
            rb.isKinematic = true;
        }

        // we set the objects parent to this controller
        objectInHand.transform.SetParent(transform);

        // if we are holding a ball, we have to set its "isBeingHeld" ball to true
        // this is so the ball knows when it is being held by the user
        if (objectInHand.GetComponent<Ball>())
        {
            objectInHand.GetComponent<Ball>().isBeingHeld = true;
        }

        // we null out the colliding object, since it is now attached to the controller
        collidingObject = null;

        // we check that the object in hand is not the ball, so that 
        // we can remove their colliders, so that the puzzle objects do not
        // collide with other objects.
        if (!objectInHand.tag.Equals("Throwable"))
        {
            // toggle colliders off while holding an object
            ToggleColliders(objectInHand, false);
        }
    }

    // this function releases the object currently being held by the controller (if any)
    private void ReleaseObject(Vector3 velocity, Vector3 angularVelocity, bool isKinematic)
    {
        // check if we are holding the ball, if so, we set it's flag
        if (objectInHand.GetComponent<Ball>())
        {
            // ball is no longer being held
            objectInHand.GetComponent<Ball>().isBeingHeld = false;
        }

        // turn the colliders back on
        ToggleColliders(objectInHand, true);

        // unparent the object
        objectInHand.transform.SetParent(null);

        // cache the rigid body
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

        // if rigidbody is found, then we set its isKinematic value (only false for the ball, true for puzzle pieces)
        // and we apply the appropiate velocities to the items (zero for puzzle pieces). 
        if (rb != null) {
            rb.isKinematic = isKinematic;
            rb.velocity = velocity * throwForce;
            rb.angularVelocity = angularVelocity;
        }

        // null out the objectInHand, since we are not holding it anymore
        objectInHand = null;
    }

    // this function is used to toggle colliders on/off for gameobjects
    // it is an iterative function, so that it also toggles any colliders that might
    // be on any of its children and its childrens children and so on
    public static void ToggleColliders(GameObject obj, bool toggle)
    {
        // if the object has a collider, toggle it
        if (obj.GetComponent<Collider>())
        {
            obj.GetComponent<Collider>().enabled = toggle;
        }

        // cache the transform of the object and determine the number of children
        Transform tempTransform = obj.transform;
        int childCount = obj.transform.childCount;

        // no more children to look for colliders, return
        if (childCount == 0)
        {
            return;
        }

        // the object has children, so we iterate through each one
        for (int i = 0; i < childCount; i++)
        {
            // cache the gameobject
            GameObject go = tempTransform.GetChild(i).gameObject;

            // check if the child has a collider. If so, toggle it.
            if (go.GetComponent<Collider>())
            {
                go.GetComponent<Collider>().enabled = toggle;
            }

            // toggle colliders in any other children
            if (go.transform.childCount > 0)
            {
                ToggleColliders(go, toggle);
            }

            go = null;
        }
    }
}
