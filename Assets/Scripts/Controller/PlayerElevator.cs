using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElevator : MonoBehaviour {

    private ControllerInputManager m_input_manager;

    public Transform playerPosition;
    public float roofHeight = 5.0f;
    public float floorHeight = 0.0f;
    public float heightToMove = 0.5f;

    private void Awake()
    {
        m_input_manager = GetComponentInParent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        m_input_manager.TouchpadPressed += new InputEventHandler(HandleTouchPadPress);
    }

    private void OnDisable()
    {
        m_input_manager.TouchpadPressed -= new InputEventHandler(HandleTouchPadPress);
    }

    private void HandleTouchPadPress(InputEventArgs e) {
        // we only want it active on the right controller
        if (e.type != ControllerType.Right)
        {
            return;
        }

        if (e.padY > 0.7)
        {
            RaisePlayer();
        }
        else if (e.padY < -0.7) {
            LowerPlayer();
        }
    }

    private void RaisePlayer() {
        if (playerPosition.position.y <= roofHeight) {
            playerPosition.Translate(Vector2.up * 0.5f);
        }
    }

    private void LowerPlayer() {
        if (playerPosition.position.y >= floorHeight) {
            playerPosition.Translate(Vector2.down * 0.5f);
        }
    }
}
