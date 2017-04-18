using UnityEngine;

// this class is used to manage the tutorial scene
public class TutorialManager : MonoBehaviour {

    // singleton
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

    public GameObject player; // reference to the player

    // reference to the tutorial stages (they are all in the same scene)
    public GameObject Tutorial1_Stage;
    public GameObject Tutorial2_Stage;
    public GameObject Tutorial3_Stage;
    public GameObject Tutorial4_Stage;
    public GameObject Tutorial5_Stage;
    public GameObject Tutorial6_Stage;
    public GameObject Tutorial7_Stage;

    // for the tutorial, initally all scripts will be disabled (except for the teleporter)
    // as the player progresses, the scripts will be activated, therefore, we need references to them
    public Teleporter teleportScript;
    public ControllerGrabObject grabScriptL;
    public ControllerGrabObject grabScriptR;
    public PlayerElevator elevatorScript;
    public ControllerObjectMenu menuScript;

    // sounds for success and failure
    public AudioClip correctSFX;
    public AudioClip incorrectSFX;

    private SteamVR_LoadLevel levelLoader; // reference to the SteamVR_LevelLoader component/script
    private Vector3 playerStartPos; // the players start position
    private AudioSource audioSource; // reference to the audiosource component

    // all the different states during the tutorial
    public enum TutorialState {
        Teleport, Elevate, Grabbing, Spawn_MetalPlank, Spawn_WoodPlank, Spawn_Funnel, Spawn_Trampoline, Spawn_Portal, Complete
    }

    // the current state we are in
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

    // this function sets the next state for our state machine
    public void SetState(TutorialState newState) {

        // set the new current state
        currTutState = newState;

        // play success sound, we have made progress
        audioSource.PlayOneShot(correctSFX);

        // then we enable the next stages of the tutorial based on the current state
        switch (currTutState)
        {
            case TutorialState.Elevate:
                elevatorScript.enabled = true;
                break;
            case TutorialState.Grabbing:
                Tutorial1_Stage.SetActive(false);
                Tutorial2_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                grabScriptL.enabled = true;
                grabScriptR.enabled = true;
                break;
            case TutorialState.Spawn_MetalPlank:
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
            case TutorialState.Spawn_Trampoline:
                Tutorial5_Stage.SetActive(false);
                Tutorial6_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                menuScript.objects[3].count = 1;
                break;
            case TutorialState.Spawn_Portal:
                Tutorial6_Stage.SetActive(false);
                Tutorial7_Stage.SetActive(true);
                player.transform.position = playerStartPos;
                menuScript.objects[4].count = 1;
                break;
            case TutorialState.Complete:
                levelLoader.Trigger();
                break;
            default:
                break;
        }
    }

    // used by other scripts to get the current state
    public TutorialState CurrentState {
        get { return currTutState; }
    }

    // used by other scripts to play failure sound
    public void PlayIncorrectSound()
    {
        audioSource.PlayOneShot(incorrectSFX);
    }
}
