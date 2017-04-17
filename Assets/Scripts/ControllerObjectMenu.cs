using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RubeObject {
    public GameObject menuPlaceholder;
    public GameObject prefab;
    public string name;
    public int count;
}

public class ControllerObjectMenu : MonoBehaviour {

    public GameObject objectMenuUI;
    // public List<GameObject> objects;
    // public ArrayList<GameObject, GameObject, int> objectList;
    public List<RubeObject> objects;

    public Text nameText;
    public Text countText;

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
            ControllerGrabObject.ToggleColliders(o.menuPlaceholder, false);
            /*
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
            */
        }

        // set the text
        SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);

        // make sure the object menu spawns a little forward of the controller
        objectMenuUI.transform.localPosition = new Vector3(0f, 0f, 0.5f);
    }

    private void HandleTouchDown(InputEventArgs e) {
        // we only want to activate the menu when the user is touching the left or right sides of the touchpad
        // print("Touchpad being touched at: (" + e.padX + " ," + e.padY + ").");

        if (e.padX > 0.7 || e.padX < -0.7)
        {
            objectMenuUI.SetActive(true);
            isMenuActive = true;
        }
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
            // Debug.Log("Should spawn object!");
            SpawnCurrentMenuObject();
        }
    }

    private void SpawnCurrentMenuObject() {

        // check that we can spawn specific item
        if (objects[currMenuIndex].count > 0)
        {
            // Instantiate the prefab
            GameObject go = Instantiate(objects[currMenuIndex].prefab, objectMenuUI.transform.position, objectMenuUI.transform.rotation);

            ControllerGrabObject.ToggleColliders(go, true);

            // decrement the count
            objects[currMenuIndex].count--;

            // set the text
            SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);

            /*
            if (go.tag.Equals("Portal"))
            {
                print("spawned a portal!");
                ToggleColliders(go, true);

            } else if (go.GetComponent<Collider>()) {
                go.GetComponent<Collider>().enabled = true;
            }
            */
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

        // set the text
        SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);
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

        // set text
        SetUIText(objects[currMenuIndex].name, objects[currMenuIndex].count);
    }

    private void SetUIText(string name, int count) {
        nameText.text = name;
        countText.text = count.ToString();
    }
}
