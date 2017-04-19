using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trampline
// This class is used to spawn the trampoline at the appropiate rotation. 
// For some reason it does not spawn at the required rotation even when rotating the prefab.
public class Trampoline : MonoBehaviour {

    private void Awake()
    {
        // rotate it!
        transform.Rotate(-90f, 0f, 0f);
    }
}
