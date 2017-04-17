using UnityEngine;

public class Ball : MonoBehaviour {

    protected Vector3 startPosition;
    protected Transform m_transform;
    protected Rigidbody m_rb;

    protected MeshRenderer m_renderer;
    protected Color originalColor;

    public bool isBeingHeld = false;

    protected bool isInvalid = false;

	// Use this for initialization
	void Start () {
        m_transform = GetComponent<Transform>();
        m_rb = GetComponent<Rigidbody>();
        startPosition = m_transform.position;
        m_renderer = GetComponent<MeshRenderer>();
        originalColor = m_renderer.material.color;
	}

    protected virtual void ResetBall() {
        m_transform.position = startPosition;

        m_renderer.material.color = originalColor;

        if (m_rb != null) {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
        isInvalid = false;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
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
        if (collision.gameObject.tag.Equals("Ground")) {
            ResetBall();
        }
    }
}
