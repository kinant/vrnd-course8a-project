using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
        /*
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        */
        currStarCount = 0;

    }

    private int numberOfStars = 0;
    public List<GameObject> stars;
    public AudioClip correctSFX;
    public AudioClip incorrectSFX;
    public AudioClip starCollectSFX;
    public bool levelIsTutorial = false;

    private int currStarCount = 0;
    private SteamVR_LoadLevel levelLoader;
    private AudioSource audioSource;

    private bool didWin = false;

	// Use this for initialization
	void Start () {
        levelLoader = GetComponent<SteamVR_LoadLevel>();
        audioSource = GetComponent<AudioSource>();
        currStarCount = 0;
        numberOfStars = stars.Count;
        didWin = false;
	}

    public void CollectStar() {
        audioSource.PlayOneShot(starCollectSFX);
        currStarCount++;
    }

    public void CheckWin() {

        print("CHECKING IF WON! curr: " + currStarCount + ", num: " + numberOfStars);

        if (currStarCount == numberOfStars)
        {
            didWin = true;
            Win();
        }
        else {
            audioSource.PlayOneShot(incorrectSFX);
        }
    }

    private void Win() {
        print("LEVEL COMPLETE! ALL STARS COLLECTED!");
        audioSource.PlayOneShot(correctSFX);
        levelLoader.Trigger();
    }

    public void ResetLevel() {
        if (!didWin && !levelIsTutorial)
        {
            audioSource.PlayOneShot(incorrectSFX);
        }

        foreach (GameObject star in stars) {
            star.SetActive(true);
        }

        currStarCount = 0;
    }

    public void PlayIncorrectSound() {
        if (!didWin && !levelIsTutorial)
        {
            audioSource.PlayOneShot(incorrectSFX);
        }
    }

}
