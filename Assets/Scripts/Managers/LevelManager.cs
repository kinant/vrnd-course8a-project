using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

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
        audioSource.PlayOneShot(correctSFX);
        levelLoader.Trigger();
    }

    public void ResetLevel() {
        if (!didWin)
        {
            audioSource.PlayOneShot(incorrectSFX);
        }

        foreach (GameObject star in stars) {
            star.SetActive(true);
        }

        currStarCount = 0;
    }

    public void PlayIncorrectSound() {
        if (!didWin)
        {
            audioSource.PlayOneShot(incorrectSFX);
        }
    }
}
