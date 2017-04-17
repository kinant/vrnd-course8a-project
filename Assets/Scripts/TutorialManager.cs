using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    private static TutorialManager _instance;

    public static TutorialManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public GameObject player;

    public GameObject Tutorial1_Stage;
    public GameObject Tutorial2_Stage;
    public GameObject Tutorial3_Stage;
    public GameObject Tutorial4_Stage;
    public GameObject Tutorial5_Stage;
    public GameObject Tutorial6_Stage;

    public Teleporter teleportScript;
    public ControllerGrabObject grabScriptL;
    public ControllerGrabObject grabScriptR;
    public PlayerElevator elevatorScript;
    public ControllerObjectMenu menuScript;

    public AudioClip correctSFX;

    private SteamVR_LoadLevel levelLoader;
    private Vector3 playerStartPos;
    private AudioSource audioSource;

    public enum TutorialState {
        Teleport, Elevate, Grabbing, Spawning, Spawn_WoodPlank, Spawn_Funnel, Spawn_Portal, Complete
    }

    private TutorialState currTutState = TutorialState.Teleport;

    // Use this for initialization
    void Start () {

        // make sure we are in the proper state
        currTutState = TutorialState.Teleport;

        // disable the scripts we do not want
        grabScriptL.enabled = false;
        grabScriptR.enabled = false;
        elevatorScript.enabled = false;
        menuScript.enabled = false;

        // set player starting position
        playerStartPos = player.transform.position;

        // level loader
        levelLoader = GetComponent<SteamVR_LoadLevel>();

        // get audiosource component
        audioSource = GetComponent<AudioSource>();
	}

    public void SetState(TutorialState newState) {
        currTutState = newState;

        audioSource.PlayOneShot(correctSFX);

        switch (currTutState)
        {
            case TutorialState.Elevate:
                elevatorScript.enabled = true;
                // Tutorial1_Stage.SetActive(false);
                // Tutorial2_Stage.SetActive(true);
                // player.transform.position = playerStartPos;
                break;
            case TutorialState.Grabbing:
                Tutorial1_Stage.SetActive(false);
                Tutorial2_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                grabScriptL.enabled = true;
                grabScriptR.enabled = true;
                break;
            case TutorialState.Spawning:
                Tutorial2_Stage.SetActive(false);
                Tutorial3_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                menuScript.enabled = true;
                break;
            case TutorialState.Spawn_WoodPlank:
                Tutorial3_Stage.SetActive(false);
                Tutorial4_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                menuScript.objects[1].count = 1;
                break;
            case TutorialState.Spawn_Funnel:
                Tutorial4_Stage.SetActive(false);
                Tutorial5_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                menuScript.objects[2].count = 1;
                break;
            case TutorialState.Spawn_Portal:
                Tutorial5_Stage.SetActive(false);
                Tutorial6_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                menuScript.objects[3].count = 1;
                break;
            case TutorialState.Complete:
                Debug.Log("Tutorial complete!");
                levelLoader.Trigger();
                break;
            default:
                break;
        }

    }

    public TutorialState CurrentState {
        get { return currTutState; }
    }

    // Update is called once per frame
    void Update() {

    }
}
