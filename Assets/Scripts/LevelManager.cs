using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

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

    public int numberOfStars = 0;
    public List<GameObject> stars;

    private int currStarCount = 0;
    private SteamVR_LoadLevel levelLoader;

	// Use this for initialization
	void Start () {
        levelLoader = GetComponent<SteamVR_LoadLevel>();
        currStarCount = 0;
	}

    public void CollectStar() {
        currStarCount++;
    }

    public void CheckWin() {
        if (currStarCount == numberOfStars) {
            Win();
        }
    }

    private void Win() {
        print("LEVEL COMPLETE! ALL STARS COLLECTED!");
        levelLoader.Trigger();
    }

    public void ResetLevel() {
        foreach (GameObject star in stars) {
            star.SetActive(true);
        }

        currStarCount = 0;
    }
}
