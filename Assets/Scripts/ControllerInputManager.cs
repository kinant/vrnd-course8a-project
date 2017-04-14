using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    Left, Right
}

public struct InputEventArgs
{
    public SteamVR_Controller.Device controller;
    public ControllerType controllerType;
    public float padX, padY;
}

public delegate void InputEventHandler(InputEventArgs e);

public class ControllerInputManager : MonoBehaviour {

    public ControllerType controllerType;

    private string contTypeString {
        get {
            if (controllerType == ControllerType.Left)
            {
                return "left";
            }
            else {
                return "right";
            }
        }
    }

    private SteamVR_TrackedObject trackedObj;

    public event InputEventHandler TriggerUp;
    public event InputEventHandler TriggerDown;
    public event InputEventHandler TouchPadPressed;
    public event InputEventHandler TouchPadTouchUp;
    public event InputEventHandler TouchPadTouchDown;
    public event InputEventHandler TouchPadPressUp;
    public event InputEventHandler TouchPadPressDown;

    public bool isTriggerPressed = false;
    public bool isTouchPadTouched = false;
    public bool isTouchPadPressed = false;

    private SteamVR_Controller.Device device {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }

    public SteamVR_Controller.Device controller {
        get { return device; }
    }

    public virtual void OnTriggerUp(InputEventArgs e) {
        if (TriggerUp != null) {
            TriggerUp(e);
        }
    }

    public virtual void OnTriggerDown(InputEventArgs e) {
        if (TriggerDown != null) {
            TriggerDown(e);
        }
    }

    public virtual void OnTouchPadTouchedDown(InputEventArgs e)
    {
        if (TouchPadTouchDown != null)
        {
            TouchPadTouchDown(e);
        }
    }

    public virtual void OnTouchPadTouchedUp(InputEventArgs e)
    {
        if (TouchPadTouchUp != null)
        {
            TouchPadTouchUp(e);
        }
    }


    public virtual void OnTouchPadPressed(InputEventArgs e) {
        if (TouchPadPressed != null) {
            TouchPadPressed(e);
        }
    }

    public virtual void OnTouchPadPressedDown(InputEventArgs e) {
        if (TouchPadPressDown != null) {
            TouchPadPressDown(e);
        }
    }

    public virtual void OnTouchPadPressedUp(InputEventArgs e)
    {
        if (TouchPadPressUp != null)
        {
            TouchPadPressUp(e);
        }
    }

    // Use this for initialization
    void Start () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();

        if (trackedObj == null) {
            trackedObj = gameObject.AddComponent<SteamVR_TrackedObject>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Check for trigger related input...
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
            //Debug.Log("Trigger is being pressed!" + contTypeString);
            isTriggerPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
            //Debug.Log("Trigger Press Down!" + contTypeString);
            InputEventArgs args = new InputEventArgs();
            args.controller = this.device;
            args.controllerType = this.controllerType;
            OnTriggerDown(args);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
            //Debug.Log("Trigger Press Up!" + contTypeString);
            InputEventArgs args = new InputEventArgs();
            args.controller = this.device;
            args.controllerType = this.controllerType;
            OnTriggerUp(args);
            isTriggerPressed = false;
        }

        // Check for touchpad related input...
        // First we check for touches
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
            //Debug.Log("Touchpad is being touched!" + contTypeString);
            isTouchPadTouched = true;
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            //Debug.Log("Touchpad Touch up!" + contTypeString);
            isTouchPadTouched = false;
            InputEventArgs args = new InputEventArgs();
            args.controller = this.device;
            args.controllerType = this.controllerType;
            OnTouchPadTouchedUp(args);
        }
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            //Debug.Log("Touchpad Touch down!" + contTypeString);
            InputEventArgs args = new InputEventArgs();
            args.controller = this.device;
            args.controllerType = this.controllerType;
            OnTouchPadTouchedDown(args);
        }

        // Then we check for presses
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
            /*
            Vector2 touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            Debug.Log("Touchpad pressed at: " + touch + ", " + contTypeString);

            InputEventArgs args = new InputEventArgs();
            args.padX = touch.x;
            args.padY = touch.y;
            OnTouchPadPressed(args);
            */
            isTouchPadPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            Vector2 touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            Debug.Log("Touchpad pressed down at: " + touch + ", " + contTypeString);

            InputEventArgs args = new InputEventArgs();
            args.controller = this.device;
            args.controllerType = this.controllerType;
            args.padY = touch.y;
            args.padX = touch.x;
            OnTouchPadPressedDown(args);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            Debug.Log("Touchpad pressed up at: " + touch + ", " + contTypeString);

            InputEventArgs args = new InputEventArgs();
            args.controller = this.device;
            args.controllerType = this.controllerType;
            args.padY = touch.y;
            args.padX = touch.x;
            OnTouchPadPressedUp(args);
            isTouchPadPressed = false;
        }
    }
}
