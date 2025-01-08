using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeView : MonoBehaviour, IInteractable {

    public string SpriteName;

    private float initialCameraSize;
    private Vector3 initialCameraPosition;

    void Awake()
    {
        initialCameraSize = Camera.main.orthographicSize;
        initialCameraPosition = Camera.main.transform.position;
    }

    public void Interact(DisplayImage currentDisplay)
    {

        currentDisplay.GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Sprites/" + SpriteName);
        currentDisplay.CurrentState = DisplayImage.State.ChangedView;

        Camera.main.orthographicSize = initialCameraSize;
        Camera.main.transform.position = initialCameraPosition;


    }

  

}
