using System.Collections.Generic;
using UnityEngine;

// This class is used to manage each level (each level has it's own, which is why we do not use a singleton)
public class LevelManager : MonoBehaviour {

    
    public List<GameObject> stars; // a list of all the collectible star gameobjects in the level
    public AudioClip correctSFX; // sound to play for success
    public AudioClip incorrectSFX; // sound to play for any failure
    public AudioClip starCollectSFX; // sound to play if the star is collected
    public GameObject solution;
    public bool showSolution = false;

    private int numberOfStars = 0; // the total number of stars in the scene to be collected
    private int currStarCount = 0; // the count of the current number of stars collected
    private SteamVR_LoadLevel levelLoader; // a reference to the SteamVR_LevelLoader script
    private AudioSource audioSource; // a reference to the gameobjects audiosource (to play sounds)

    private bool didWin = false; // flag that is set when the player wins the level

	// Use this for initialization
	void Start () {
        // get components
        levelLoader = GetComponent<SteamVR_LoadLevel>();
        audioSource = GetComponent<AudioSource>();

        // make sure the current star count is 0
        currStarCount = 0;

        // get the number of stars in the level
        numberOfStars = stars.Count;

        // make sure we have not won yet
        didWin = false;

        // show the solution?
        solution.SetActive(showSolution);
    }

    // this function is called by the Ball when a star is collected
    public void CollectStar() {

        // play the sound
        audioSource.PlayOneShot(starCollectSFX);

        // increase the count
        currStarCount++;
    }

    // this function is called when we want to check if we have won the level
    public void CheckWin() {

        // check that we have collected the right amount of stars
        if (currStarCount == numberOfStars)
        {
            // if so, we win
            didWin = true;
            Win();
        }
        else {
            // if not, we play the failure song
            audioSource.PlayOneShot(incorrectSFX);
        }
    }

    // function is called when we win
    private void Win() {

        // play sound and load next level
        audioSource.PlayOneShot(correctSFX);
        levelLoader.Trigger();
    }

    // this function is called to reset the level
    public void ResetLevel() {

        // if we havent won when reseting, we play failure song (ball has probably hit the ground)
        if (!didWin)
        {
            audioSource.PlayOneShot(incorrectSFX);
        }

        // activate all the stars
        foreach (GameObject star in stars) {
            star.SetActive(true);
        }

        // reset the current star collect count
        currStarCount = 0;
    }

    // this function is called by other scripts to play the failure song
    public void PlayIncorrectSound() {
        if (!didWin)
        {
            audioSource.PlayOneShot(incorrectSFX);
        }
    }
}
