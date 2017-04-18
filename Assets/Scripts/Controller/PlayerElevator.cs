using UnityEngine;

// this class is used to add up/down movement to the player. The player can teleport to anywhere on the ground,
// but to reach higher or lower parts of the scene, they will need to press the right controllers up/down touchpad.
public class PlayerElevator : MonoBehaviour {

    private ControllerInputManager m_input_manager; // the controller input manager

    public Transform playerPosition; // a reference to the players position
    public float roofHeight = 5.0f; // max height the player can go up
    public float floorHeight = 0.0f; // lowest height the player can go down
    public float heightToMove = 0.5f; // the amount of vertical distance to move 

    private void Awake()
    {
        // get the controller input manager
        m_input_manager = GetComponentInParent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        // subscribe to controller events
        m_input_manager.TouchpadPressed += new InputEventHandler(HandleTouchPadPress);
    }

    private void OnDisable()
    {
        // unsubscribe from controller events
        m_input_manager.TouchpadPressed -= new InputEventHandler(HandleTouchPadPress);
    }

    // handles the event for when the controllers touchpad is pressed
    private void HandleTouchPadPress(InputEventArgs e) {
        // we only want it active on the right controller
        if (e.type != ControllerType.Right)
        {
            return;
        }

        // did we press the pad UP?
        if (e.padY > 0.7)
        {
            // we raise the player
            RaisePlayer();
        }
        // or did we press the pad DOWN?
        else if (e.padY < -0.7) {

            // we lower the player
            LowerPlayer();
        }
    }

    // this function raises the player
    private void RaisePlayer() {

        // check that the player is not too high, then move
        if (playerPosition.position.y <= roofHeight) {
            playerPosition.Translate(Vector2.up * 0.5f);
        }
    }

    // this function lowers the player
    private void LowerPlayer() {

        // check that the player is not too low, then move
        if (playerPosition.position.y >= floorHeight) {
            playerPosition.Translate(Vector2.down * 0.5f);
        }
    }
}
