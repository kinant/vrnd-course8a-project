using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this Serializable class is used so that we have an organized way of setting the menu items
// as it is serializable, it will show up in the inspector
[Serializable]
public class RubeObject {
    public GameObject menuPlaceholder; // placeholder for the menu item
    public GameObject prefab; // the prefab it will spawn
    public string name; // name of the object
    public int count; // the count (number of objects we can spawn)
}

// This class handles the menu for viewing and spawning puzzle pieces/objects
public class ControllerObjectMenu : MonoBehaviour {

    public GameObject objectMenuUI; // reference to the actual menu gameobject
    public List<RubeObject> objects; // a list of all the puzzle objects

    public Text nameText; // a reference to the UI text that will show the name of the current object being viewed
    public Text countText; // as above, but will be used to show the current count of objects we can spawn

    private ControllerInputManager m_input_manager; // reference to the controller input manager
    private int currMenuIndex = 0; // the current index for the currently viewed menu item
    private bool isMenuActive = false; // flag for telling if the menu is active

    private void Awake()
    {
        // get the controller input manager
        m_input_manager = GetComponentInParent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        // subscribe to controller events
        m_input_manager.TouchpadTouched += new InputEventHandler(HandleTouchDown);
        m_input_manager.TouchpadUntouched += new InputEventHandler(HandleTouchUp);
        m_input_manager.TouchpadPressed += new InputEventHandler(HandleTouchPress);
        m_input_manager.TriggerPressed += new InputEventHandler(HandleTriggerDown);
    }

    private void OnDisable()
    {
        // unsubscribe from controller events
        m_input_manager.TouchpadTouched -= new InputEventHandler(HandleTouchDown);
        m_input_manager.TouchpadUnpressed -= new InputEventHandler(HandleTouchUp);
        m_input_manager.TouchpadPressed -= new InputEventHandler(HandleTouchPress);
        m_input_manager.TriggerPressed -= new InputEventHandler(HandleTriggerDown);
    }

    private void Start()
    {
        // make sure current index is 0
        currMenuIndex = 0;

        // we disable all the colliders on the menu objects
        foreach (RubeObject o in objects) {
            ControllerGrabObject.ToggleColliders(o.menuPlaceholder, false);
        }

        // set the text
        SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);

        // make sure the object menu spawns a little forward of the controller
        objectMenuUI.transform.localPosition = new Vector3(0f, 0f, 0.65f);
    }

    // handle the controller's touchpad being touched
    private void HandleTouchDown(InputEventArgs e) {
        // we only want it active on the right controller
        if (e.type != ControllerType.Right)
        {
            return;
        }

        // we only want to activate the menu when the user is touching the left or right sides of the touchpad
        if (e.padX > 0.5 || e.padX < -0.5)
        {
            objectMenuUI.SetActive(true);
            isMenuActive = true;
        }
    }

    // handle the event that the touchpad is no longer being touched
    private void HandleTouchUp(InputEventArgs e) {
        // we only want it active on the right controller
        if (e.type != ControllerType.Right)
        {
            return;
        }

        // disable the menu
        objectMenuUI.SetActive(false);
        isMenuActive = false;
    }


    // handle presses on the touchpad
    private void HandleTouchPress(InputEventArgs e) {
        // we only want it active on the right controller
        if (e.type != ControllerType.Right)
        {
            return;
        }
        // pad pressed right
        if (e.padX > 0.5)
        {
            // move on to next menu item
            MenuNext();
        }
        // pad pressed left
        else if (e.padX < -0.5) {

            // move back to previous menu item
            MenuPrevious();
        }
    }

    // handle the event for when the controller's trigger is pressed
    private void HandleTriggerDown(InputEventArgs e)
    {
        // we only want it active on the left controller
        if (e.type != ControllerType.Right)
        {
            return;
        }

        // we only spawn an object when the menu is active
        if (isMenuActive) {
            SpawnCurrentMenuObject();
        }
    }

    // this function is used to spawn the object that is being shown on the menu (if active)
    private void SpawnCurrentMenuObject() {

        // check that we can spawn specific item
        if (objects[currMenuIndex].count > 0)
        {
            // Instantiate the prefab
            GameObject go = Instantiate(objects[currMenuIndex].prefab, objectMenuUI.transform.position, objectMenuUI.transform.rotation);

            // turn on the colliders
            ControllerGrabObject.ToggleColliders(go, true);

            // decrement the count
            objects[currMenuIndex].count--;

            // set the text
            SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);
        }
    }

    // function to show the next menu item
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

        // set the text
        SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);
    }

    // function to show the previous menu item
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

        // set text
        SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);
    }

    // function to set the UI text elements
    private void SetUIText(string name, int count) {
        nameText.text = name;
        countText.text = count.ToString();
    }
}
