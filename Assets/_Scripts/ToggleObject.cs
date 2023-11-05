using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public bool initialState;

    private void Start()
    {
        // Optional: You can set the initial state of the GameObject here if needed.
        gameObject.SetActive(initialState);
    }

    public void ToggleGameObject()
    {
        if (gameObject.activeSelf) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
