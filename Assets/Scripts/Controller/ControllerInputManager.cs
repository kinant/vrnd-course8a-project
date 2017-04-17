using UnityEngine;

public enum ControllerType
{
    Left, Right, None
}

public struct InputEventArgs
{
    public SteamVR_Controller.Device controller;
    public ControllerType type;
    public float flags;
    public float padX, padY;
}

public delegate void InputEventHandler(InputEventArgs e);

public class ControllerInputManager : MonoBehaviour
{

    [SerializeField] private SteamVR_TrackedObject controllerL_trackedObj;
    [SerializeField] private SteamVR_TrackedObject controllerR_trackedObj;

    private SteamVR_Controller.Device controllerL_device
    {
        get { return SteamVR_Controller.Input((int)controllerL_trackedObj.index); }
    }

    private SteamVR_Controller.Device controllerR_device
    {
        get { return SteamVR_Controller.Input((int)controllerR_trackedObj.index); }
    }

    public ControllerType GetControllerType(SteamVR_Controller.Device d)
    {
        if (d == controllerL_device)
        {
            return ControllerType.Left;
        }
        if (d == controllerR_device)
        {
            return ControllerType.Right;
        }

        return ControllerType.None;
    }

    // flags for inputs
    private bool _triggerPressed = false;
    private bool _triggerTouched = false;
    private bool _touchpadTouched = false;
    private bool _gripPressed = false;
    private bool _menuPressed = false;
    private bool _systemPressed = false;

    public bool triggerPressed
    {
        get { return _triggerPressed; }
    }

    public bool triggerTouched
    {
        get { return _triggerTouched; }
    }

    public bool touchpadTouched
    {
        get { return _touchpadTouched; }
    }

    // event handlers for inputs
    public event InputEventHandler TriggerPressed;
    public event InputEventHandler TriggerUnpressed;
    public event InputEventHandler TouchpadPressed;
    public event InputEventHandler TouchpadUnpressed;
    public event InputEventHandler TouchpadTouched;
    public event InputEventHandler TouchpadUntouched;
    public event InputEventHandler GripPressed;
    public event InputEventHandler GripUnpressed;
    public event InputEventHandler MenuPressed;
    public event InputEventHandler MenuUnpressed;
    public event InputEventHandler SystemPressed;
    public event InputEventHandler SystemUnpressed;

    void OnControllerInput(InputEventHandler handler, InputEventArgs e)
    {
        if (handler != null)
        {
            handler(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerL_device != null)
        {
            CheckForInput(controllerL_device);
        }

        if (controllerR_device != null)
        {
            CheckForInput(controllerR_device);
        }
    }

    void CheckForInput(SteamVR_Controller.Device device)
    {

        Vector2 touch = Vector2.zero;

        // set flag if trigger is being pressed
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            _triggerPressed = true;
        }

        // check for trigger presses:
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            SendEvent(TriggerPressed, device, 0, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            SendEvent(TriggerUnpressed, device, 0, 0, 0);
        }

        // set flag if touchpad is being touched
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            _touchpadTouched = true;
        }

        // check for touchpad touches
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            SendEvent(TouchpadTouched, device, 0, touch.x, touch.y);
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            SendEvent(TouchpadUntouched, device, 0, touch.x, touch.y);
        }

        // check for touchpad presses
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            SendEvent(TouchpadPressed, device, 0, touch.x, touch.y);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            SendEvent(TouchpadUnpressed, device, 0, touch.x, touch.y);
        }

        // check for grip presses
        if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            _gripPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            SendEvent(GripPressed, device, 0, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            _gripPressed = false;
            SendEvent(GripUnpressed, device, 0, 0, 0);
        }

        // check for Menu button presses
        if (device.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            _menuPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            SendEvent(MenuPressed, device, 0, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            SendEvent(MenuUnpressed, device, 0, 0, 0);
        }

        // check for System button presses
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.System))
        {
            SendEvent(SystemPressed, device, 0, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.System))
        {
            SendEvent(SystemUnpressed, device, 0, 0, 0);
        }
    }

    public void SendEvent(InputEventHandler handler, SteamVR_Controller.Device cont, float flags, float padX, float padY)
    {
        InputEventArgs args = new InputEventArgs();
        args.controller = cont;
        args.type = GetControllerType(cont);
        args.flags = flags;
        args.padX = padX;
        args.padY = padY;

        OnControllerInput(handler, args);
    }
}