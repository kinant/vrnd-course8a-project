using UnityEngine;

// a basic ball
public class Ball : MonoBehaviour {

    protected Vector3 startPosition; // ball's start position
    protected Transform m_transform; // we will cache the transform
    protected Rigidbody m_rb; // we will cache the rigidbody

    protected MeshRenderer m_renderer; // we will cache the mesh renderer
    protected Color originalColor; // we will keep a reference of the balls original color (it turns black)

    public bool isBeingHeld = false; // a flag for the ball to know if it is being held by the user

    protected bool isInvalid = false; // a flag to know if the ball is invalid. It will be invalid if the user is holding it
                                      // and they leave the play area holding the ball

	// Use this for initialization
	void Start () {

        // set the cached references
        m_transform = GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody>();
        startPosition = m_transform.position;
        m_renderer = GetComponent<MeshRenderer>();
        originalColor = m_renderer.material.color;
	}

    // resets the ball
    protected virtual void ResetBall() {
        m_transform.position = startPosition;

        m_renderer.material.color = originalColor;

        if (m_rb != null) {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
        isInvalid = false;
    }

    // handle ball collisions
    protected virtual void OnTriggerExit(Collider other)
    {
        // if the ball is being held and it leaves the play area, the player is cheating
        // so we invalidate the ball
        if (other.gameObject.tag.Equals("PlayArea")) {
            if (isBeingHeld)
            {
                // Player has exited the play area with the ball, invalidate the ball
                m_renderer.material.color = Color.black;
                isInvalid = true;
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // if the ball hits the ground, we reset it
        if (collision.gameObject.tag.Equals("Ground")) {
            ResetBall();
        }
    }
}
