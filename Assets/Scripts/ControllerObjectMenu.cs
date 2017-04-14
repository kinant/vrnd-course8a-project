using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RubeObject {
    public GameObject menuPlaceholder;
    public GameObject prefab;
    public int count;
}

public class ControllerObjectMenu : MonoBehaviour {

    public GameObject objectMenuUI;
    // public List<GameObject> objects;
    // public ArrayList<GameObject, GameObject, int> objectList;
    public List<RubeObject> objects;

    private ControllerInputManager m_input_manager;
    private int currMenuIndex = 0;
    private bool isMenuActive = false;

    private void Awake()
    {
        m_input_manager = GetComponent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        m_input_manager.TouchPadTouchDown += new InputEventHandler(HandleTouchDown);
        m_input_manager.TouchPadTouchUp += new InputEventHandler(HandleTouchUp);
        m_input_manager.TouchPadPressDown += new InputEventHandler(HandleTouchPress);
        m_input_manager.TriggerDown += new InputEventHandler(HandleTriggerDown);
    }

    private void OnDisable()
    {
        m_input_manager.TouchPadTouchDown -= new InputEventHandler(HandleTouchDown);
        m_input_manager.TouchPadTouchUp -= new InputEventHandler(HandleTouchUp);
        m_input_manager.TouchPadPressDown -= new InputEventHandler(HandleTouchPress);
        m_input_manager.TriggerDown -= new InputEventHandler(HandleTriggerDown);
    }

    private void Start()
    {
        // make sure current index is 0
        currMenuIndex = 0;

        // we disable all the colliders on the menu objects
        foreach (RubeObject o in objects) {
            // check if we have a portal or not
            if (o.menuPlaceholder.transform.GetChild(0).gameObject.tag.Equals("Portal"))
            {
                TogglePortalColliders(o.menuPlaceholder, false);
            }
            else
            {
                Collider col = o.menuPlaceholder.GetComponentInChildren<Collider>();
                if (col != null)
                {
                    col.enabled = false;
                }
            }
        }

        // make sure the object menu spawns a little forward of the controller
        objectMenuUI.transform.localPosition = new Vector3(0f, 0f, 0.5f);
    }

    private void TogglePortalColliders(GameObject portal, bool toggle) {

        print("checking parent: " + portal.name);
        Transform portalTransform = portal.transform;
        int childCount = portal.transform.childCount;
        print("this parent has: " + childCount + " children.");

        for (int i = 0; i < childCount; i++) {
            GameObject go = portalTransform.GetChild(i).gameObject;
            print("checking child: " + go.name);

            if (go.GetComponent<Collider>()) {
                print("this child has a colider!");
                go.GetComponent<Collider>().enabled = toggle;
            }

            if (go.transform.childCount > 0) {
                TogglePortalColliders(go, toggle);
            }
        }
    }

    private void HandleTouchDown(InputEventArgs e) {
        objectMenuUI.SetActive(true);
        isMenuActive = true;
    }

    private void HandleTouchUp(InputEventArgs e) {
        objectMenuUI.SetActive(false);
        isMenuActive = false;
    }

    private void HandleTouchPress(InputEventArgs e) {
        // pad pressed right
        if (e.padX > 0.7)
        {
            MenuNext();
        }
        // pad pressed left
        else if (e.padX < -0.7) {
            MenuPrevious();
        }
    }

    private void HandleTriggerDown(InputEventArgs e)
    {
        if (isMenuActive) {
            Debug.Log("Should spawn object!");
            SpawnCurrentMenuObject();
        }
    }

    private void SpawnCurrentMenuObject() {
        // Instantiate the prefab
        GameObject go = Instantiate(objects[currMenuIndex].prefab, objectMenuUI.transform.position, objectMenuUI.transform.rotation);

        if (go.tag.Equals("Portal"))
        {
            print("spawned a portal!");
            TogglePortalColliders(go, true);

        } else if (go.GetComponent<Collider>()) {
            go.GetComponent<Collider>().enabled = true;
        }
    }

    private void MenuNext() {
        // we deactivate the current menu object
        objects[currMenuIndex].menuPlaceholder.SetActive(false);

        // we increment the counter
        currMenuIndex++;

        // we check if we have incremented too high
        if (currMenuIndex > objects.Count - 1) {
            // reset the counter
            currMenuIndex = 0;
        }

        // activate the new menu item
        objects[currMenuIndex].menuPlaceholder.SetActive(true);
    }

    private void MenuPrevious() {
        // we deactivate the current menu object
        objects[currMenuIndex].menuPlaceholder.SetActive(false);

        // we decrement the counter
        currMenuIndex--;

        // we check if we have incremented too high
        if (currMenuIndex < 0)
        {
            // reset the counter
            currMenuIndex = objects.Count - 1;
        }

        // activate the new menu item
        objects[currMenuIndex].menuPlaceholder.SetActive(true);
    }
}
