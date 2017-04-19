using UnityEngine;

// this class is used to allow the player to teleport anywhere on the ground
// I used the SteamVR_LaserPointer script as a reference. 
public class Teleporter : MonoBehaviour {

    public Color color; // laser pointer color
    public float thickness = 0.002f; // laser pointer thickness
    public Transform player; // reference to player position
    public GameObject pointerIndicatorPrefab; // prefab for indicator that shows when there is a valid teleport destination
    public LayerMask layerMask; // layermask for raycast
    public Material laserPointerMaterial;

    private GameObject pointer; // the laserpointer
    private GameObject holder; // the laserpointer holder

    private bool isActive = false; // flag to tell if the laser pointer is active or not

    private GameObject pointerIndicator; // reference to the pointer indicator
    private MeshRenderer pointerIndicatorMeshRenderer; // cached reference to the pointer indicator mesh renderer

    private Transform pointerTransform; // cached reference to the laser pointers transform
    private Transform pointerIndicatorTransform; // cached reference to the laser pointers' indicator transform

    private Transform currContact = null; // cache for the transform of any current contact (by the raycast)
    private bool isCurrContact = false; // flag to tell if we currenty have a contact

    private ControllerInputManager m_input_manager; // reference to the controller input manager

    private Transform m_transform; // reference for the controllers transform

    private void Awake()
    {
        // get the controller input manager
        m_input_manager = GetComponentInParent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        // subscribe to controller events
        m_input_manager.TouchpadPressed += new InputEventHandler (ActivateTeleporter);
        m_input_manager.TouchpadUnpressed += new InputEventHandler(DeactivateTeleporter);
    }

    private void OnDisable()
    {
        // unsubscribe to controller events
        m_input_manager.TouchpadPressed -= new InputEventHandler(ActivateTeleporter);
        m_input_manager.TouchpadUnpressed -= new InputEventHandler(DeactivateTeleporter);
    }

    // Event Handlers...
    // We activate the teleporter when the touchpad is pressed
    private void ActivateTeleporter(InputEventArgs e)
    {
        // we only want it active on the left controller
        if (e.type != ControllerType.Left) {
            return;
        }

        // activate
        if (pointer != null)
        {
            pointer.SetActive(true);
            isActive = true;

            // activate the indicator if we had a current contact before
            if (currContact != null)
            {
                EnablePointerIndicator();
            }
        }
    }

    // We deactivate the teleporter when the touchpad is released
    private void DeactivateTeleporter(InputEventArgs e)
    {
        // we only want it active on the left controller
        if (e.type != ControllerType.Left)
        {
            return;
        }

        // before we deactivate, we will teleport the player, but only if there is a current contact
        if (isCurrContact)
        {
            // teleport to the location
            player.position = pointerIndicator.transform.position;
            isCurrContact = false;
            currContact = null;
        }

        // deactivate
        if (pointer != null)
        {
            pointer.SetActive(false);
            DisablePointerIndicator();
        }
        isActive = false;
    }


    // Use this for initialization
    void Start()
    {
        // create the laser pointer holder
        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity;

        // create the laser pointer
        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.name = "Pointer";
        pointer.transform.parent = holder.transform;
        pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
        pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
        pointer.transform.localRotation = Quaternion.identity;

        // Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        // newMaterial.SetColor("_Color", color);
        pointer.GetComponent<MeshRenderer>().material = laserPointerMaterial;

        // check if we have a prefab for the pointer indicator, if so, we create it
        if (pointerIndicatorPrefab != null)
        {
            pointerIndicator = GameObject.Instantiate(pointerIndicatorPrefab);
            pointerIndicatorMeshRenderer = pointerIndicator.GetComponent<MeshRenderer>();

            // we initially do not want to see the indicator, so we disable its renderer
            pointerIndicatorMeshRenderer.enabled = false;

            // cache the transform
            pointerIndicatorTransform = pointerIndicator.transform;
        }

        // cache pointer transform
        pointerTransform = pointer.transform;

        // cache controller transform
        m_transform = GetComponent<Transform>();

        // if the pointer has any sort of collider, we want to remove it
        BoxCollider collider = pointer.GetComponent<BoxCollider>();

        if (collider)
        {
            UnityEngine.Object.Destroy(collider);
        }

        if (!isActive)
        {
            pointer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if the laser pointer is not active, do nothing
        if (!isActive)
        {
            return;
        }

        float dist = 100f; // laser pointer distance
        float thick = thickness; // laser pointer thickness

        // we create a ray, and we use physics raycast to detect any contact with the laser pointer and the floor, which
        // is defined by the layermask

        // Debug.Log("transform: " + transform.position + ", fwd:" + transform.forward);

        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        bool bHit = Physics.Raycast(raycast, out hit, 100f, layerMask);

        // if we had any sort of hit
        if (bHit)
        {
            // cache the hit transform
            Transform hitTransform = hit.transform;

            // handle new hit...
            // we check that we have no current contact, and that any previous contact is not the same
            if (currContact == null || (currContact != hitTransform))
            {
                // set the new current contact
                currContact = hitTransform;
                isCurrContact = true;

                // we enable the indicator
                EnablePointerIndicator();
            }

            // handle hitting the same object...
            if (currContact != null && currContact == hitTransform)
            {
                // if we are hitting the same game object, then we only move the indicator positon
                if (pointerIndicatorTransform != null)
                {
                    pointerIndicatorTransform.position = hit.point;
                }
            }

            // if the hits distance is less than 100f, we will shorten the laser pointer
            if (hit.distance < 100f)
            {
                dist = hit.distance;
            }
        }
        // if there was NO hit
        else
        {
            // if we used to have a current contact (not null), then we null it and disable the pointer indicator
            if (currContact != null)
            {
                isCurrContact = false;
                currContact = null;
                DisablePointerIndicator();
            }
        }

        // set the pointers new thickness and length
        pointer.transform.localScale = new Vector3(thick, thick, dist);
        pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
    }

    // this function is used to hide the pointer indicator
    private void DisablePointerIndicator()
    {
        if (pointerIndicatorMeshRenderer != null)
        {
            pointerIndicatorMeshRenderer.enabled = false;
        }
    }

    // this function is used to show the pointer indicator
    private void EnablePointerIndicator()
    {
        if (pointerIndicatorMeshRenderer != null)
        {
            pointerIndicatorMeshRenderer.enabled = true;
        }
    }
}
