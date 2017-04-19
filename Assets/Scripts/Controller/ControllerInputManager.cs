using UnityEngine;

// enum for the controller type
public enum ControllerType
{
    Left, Right, None
}

// event arguments
public struct InputEventArgs
{
    public SteamVR_Controller.Device controller; // the device/controller that registered the input
    public ControllerType type; // the type of controller it is
    public float padX, padY; // x and y points of the touchpad axis
}

// delegate
public delegate void InputEventHandler(InputEventArgs e);


// This class uses the event/delegate pattern to handle the input by controllers
public class ControllerInputManager : MonoBehaviour
{

    // references to both controller's SteamVR_TrackedObject component/script
    [SerializeField] private SteamVR_TrackedObject controllerL_trackedObj;
    [SerializeField] private SteamVR_TrackedObject controllerR_trackedObj;

    // the devices for the left and right controllers
    private SteamVR_Controller.Device controllerL_device
    {
        get { return SteamVR_Controller.Input((int)controllerL_trackedObj.index); }
    }

    private SteamVR_Controller.Device controllerR_device
    {
        get { return SteamVR_Controller.Input((int)controllerR_trackedObj.index); }
    }

    // utility function to get the controller type
    private ControllerType GetControllerType(SteamVR_Controller.Device d)
    {

        // check if it is the left controller
        if (d == controllerL_device)
        {
            return ControllerType.Left;
        }

        // check if it is the right controller
        if (d == controllerR_device)
        {
            return ControllerType.Right;
        }

        // it is neither controller
        return ControllerType.None;
    }

    // flags for inputs
    private bool _triggerPressed = false;
    private bool _triggerTouched = false;
    private bool _touchpadTouched = false;
    private bool _touchpadPressed = false;
    private bool _gripPressed = false;
    private bool _menuPressed = false;
    private bool _systemPressed = false;

    // public getters for the previous flags
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

    // events for all the different inputs
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

    // we only use one InputEventHandler function to publish/fire all of the different events
    void OnControllerInput(InputEventHandler handler, InputEventArgs e)
    {
        // check that the handler passed in is not null
        if (handler != null)
        {
            // call it
            handler(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerL_trackedObj == null || controllerR_trackedObj == null) {
            return;
        }

        if (controllerL_device == null || controllerR_device == null)
        {
            return;
        }

        // we check for input from both devices
        if (controllerL_device != null)
        {
            CheckForInput(controllerL_device);
        }

        if (controllerR_device != null)
        {
            CheckForInput(controllerR_device);
        }
    }

    // this function goes through all the combinations of inputs to see if the controller has any sort of input
    void CheckForInput(SteamVR_Controller.Device device)
    {
        // a vector that will be used for any touchpad axis information
        Vector2 touch = Vector2.zero;

        // set flag if trigger is being pressed
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            _triggerPressed = true;
        }

        // check for trigger presses:
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            SendEvent(TriggerPressed, device, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            SendEvent(TriggerUnpressed, device, 0, 0);
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
            SendEvent(TouchpadTouched, device, touch.x, touch.y);
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            _touchpadTouched = false;
            touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            SendEvent(TouchpadUntouched, device, touch.x, touch.y);
        }

        // check for touchpad presses
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            _touchpadPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            SendEvent(TouchpadPressed, device, touch.x, touch.y);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            _touchpadPressed = false;
            touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            SendEvent(TouchpadUnpressed, device, touch.x, touch.y);
        }

        // check for grip presses
        if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            _gripPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            SendEvent(GripPressed, device, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            _gripPressed = false;
            SendEvent(GripUnpressed, device, 0, 0);
        }

        // check for Menu button presses
        if (device.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            _menuPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            SendEvent(MenuPressed, device, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            _menuPressed = false;
            SendEvent(MenuUnpressed, device, 0, 0);
        }

        // check for System button presses
        if (device.GetPress(SteamVR_Controller.ButtonMask.System))
        {
            _systemPressed = true;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.System))
        {
            SendEvent(SystemPressed, device, 0, 0);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.System))
        {
            _systemPressed = false;
            SendEvent(SystemUnpressed, device, 0, 0);
        }
    }

    // this intermediate function is used to build the arguments for the event
    public void SendEvent(InputEventHandler handler, SteamVR_Controller.Device cont, float padX, float padY)
    {
        InputEventArgs args = new InputEventArgs();
        args.controller = cont;
        args.type = GetControllerType(cont);
        args.padX = padX;
        args.padY = padY;

        OnControllerInput(handler, args);
    }
}