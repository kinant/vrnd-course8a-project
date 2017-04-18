using UnityEngine;

// the collectible star
// all this class does is rotate the star, so that it is animated
public class Star : MonoBehaviour {

    public float rotationSpeed = 95f;

    private Transform m_transform;

    // Use this for initialization
    void Start () {
        m_transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        // rotate the star
        m_transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f));
	}
}
