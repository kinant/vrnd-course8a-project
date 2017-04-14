using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObjectMenu : MonoBehaviour {

    public GameObject objectMenuUI;
    public List<GameObject> objects;

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
        foreach (GameObject o in objects) {
            Collider col = o.GetComponentInChildren<Collider>();

            if (col != null) {
                col.enabled = false;
            }
        }

        // make sure the object menu spawns a little forward of the controller
        objectMenuUI.transform.localPosition = new Vector3(0f, 0f, 0.5f);
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
        GameObject go = Instantiate(objects[currMenuIndex].transform.GetChild(0).gameObject, objects[currMenuIndex].transform.position, objects[currMenuIndex].transform.rotation);

        // Set the tag
        go.tag = "Structure";

        // Check if the prefab has its collider, if so, enable it
        if (go.GetComponent<Collider>()) {
            go.GetComponent<Collider>().enabled = true;
        }
    }

    private void MenuNext() {
        // we deactivate the current menu object
        objects[currMenuIndex].SetActive(false);

        // we increment the counter
        currMenuIndex++;

        // we check if we have incremented too high
        if (currMenuIndex > objects.Count - 1) {
            // reset the counter
            currMenuIndex = 0;
        }

        // activate the new menu item
        objects[currMenuIndex].SetActive(true);
    }

    private void MenuPrevious() {
        // we deactivate the current menu object
        objects[currMenuIndex].SetActive(false);

        // we decrement the counter
        currMenuIndex--;

        // we check if we have incremented too high
        if (currMenuIndex < 0)
        {
            // reset the counter
            currMenuIndex = objects.Count - 1;
        }

        // activate the new menu item
        objects[currMenuIndex].SetActive(true);
    }
}
