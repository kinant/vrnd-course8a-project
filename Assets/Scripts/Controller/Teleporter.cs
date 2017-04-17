using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public Color color;
    public float thickness = 0.002f;
    public Transform player;
    public GameObject pointerIndicatorPrefab;
    public LayerMask layerMask;

    private GameObject pointer;
    private GameObject holder;

    private bool isActive = false;

    private GameObject pointerIndicator;
    private MeshRenderer pointerIndicatorMeshRenderer;
    private Transform pointerTransform;
    private Transform pointerIndicatorTransform;

    private Transform currContact = null;
    private bool isCurrContact = false;

    private ControllerInputManager m_input_manager;

    private Transform m_transform;

    private void Awake()
    {
        m_input_manager = GetComponentInParent<ControllerInputManager>();
    }

    private void OnEnable()
    {
        m_input_manager.TouchpadPressed += new InputEventHandler (ActivateTeleporter);
        m_input_manager.TouchpadUnpressed += new InputEventHandler(DeactivateTeleporter);
    }

    private void OnDisable()
    {
        m_input_manager.TouchpadPressed -= new InputEventHandler(ActivateTeleporter);
        m_input_manager.TouchpadUnpressed -= new InputEventHandler(DeactivateTeleporter);
    }

    // Event Handlers...
    private void ActivateTeleporter(InputEventArgs e)
    {
        // we only want it active on the left controller
        if (e.type != ControllerType.Left) {
            return;
        }

        Debug.Log("should activate teleporter!");
        // activate
        if (pointer != null)
        {
            pointer.SetActive(true);
            isActive = true;

            if (currContact != null)
            {
                EnablePointerIndicator();
            }
        }
    }

    private void DeactivateTeleporter(InputEventArgs e)
    {
        // we only want it active on the left controller
        if (e.type != ControllerType.Left)
        {
            return;
        }

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
        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.name = "Pointer";
        pointer.transform.parent = holder.transform;
        pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
        pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
        pointer.transform.localRotation = Quaternion.identity;

        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.SetColor("_Color", color);
        pointer.GetComponent<MeshRenderer>().material = newMaterial;

        if (pointerIndicatorPrefab != null)
        {
            pointerIndicator = GameObject.Instantiate(pointerIndicatorPrefab);
            pointerIndicatorMeshRenderer = pointerIndicator.GetComponent<MeshRenderer>();
            pointerIndicatorMeshRenderer.enabled = false;
            pointerIndicatorTransform = pointerIndicator.transform;
        }

        pointerTransform = pointer.transform;

        m_transform = GetComponent<Transform>();

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
        if (!isActive)
        {
            return;
        }

        float dist = 100f;
        float thick = thickness;

        Ray raycast = new Ray(m_transform.position, m_transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, 100f, layerMask);

        if (bHit)
        {
            Transform hitTransform = hit.transform;

            // handle new hit...
            if (currContact == null || (currContact != hitTransform))
            {

                if (currContact != null)
                {
                    // OnPointerOut();
                }

                currContact = hitTransform;
                isCurrContact = true;

                // responder = currContact.gameObject.GetComponent(typeof(IPointerResponder)) as IPointerResponder;
                // OnPointerIn();
                EnablePointerIndicator();
            }

            // handle hitting the same object...
            if (currContact != null && currContact == hitTransform)
            {
                if (pointerIndicatorTransform != null)
                {
                    pointerIndicatorTransform.position = hit.point;
                }
            }
            if (hit.distance < 100f)
            {
                dist = hit.distance;
            }
        }
        else
        {
            if (currContact != null)
            {
                isCurrContact = false;
                currContact = null;
                DisablePointerIndicator();
            }
        }

        pointer.transform.localScale = new Vector3(thick, thick, dist);
        pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
    }

    private void DisablePointerIndicator()
    {
        if (pointerIndicatorMeshRenderer != null)
        {
            pointerIndicatorMeshRenderer.enabled = false;
        }
    }

    private void EnablePointerIndicator()
    {
        if (pointerIndicatorMeshRenderer != null)
        {
            pointerIndicatorMeshRenderer.enabled = true;
        }
    }
}
