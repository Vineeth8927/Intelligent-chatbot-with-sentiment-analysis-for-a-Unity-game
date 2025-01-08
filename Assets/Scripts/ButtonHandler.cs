using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {

    private DisplayImage currentDisplay;

    private float initialCameraSize;
    private Vector3 initialCameraPosition;

    private ZoomInObject[] zoomInObjects;

    void Awake()
    {
        currentDisplay = GameObject.Find("displayImage").GetComponent<DisplayImage>();
        initialCameraSize = Camera.main.orthographicSize;
        initialCameraPosition = Camera.main.transform.position;

        zoomInObjects = FindObjectsOfType<ZoomInObject>();
    }

    public void OnRightClickArrow()
    {
        if (currentDisplay.CurrentState == DisplayImage.State.idle) return;

        currentDisplay.CurrentWall = currentDisplay.CurrentWall + 1;
    }

    public void OnLeftClickArrow()
    {
        if (currentDisplay.CurrentState == DisplayImage.State.idle) return;

        currentDisplay.CurrentWall = currentDisplay.CurrentWall - 1;
    }

    public void OnClickReturn()
    {
        if (currentDisplay.CurrentState == DisplayImage.State.idle) return;

        if (currentDisplay.CurrentState == DisplayImage.State.zoom)
        {
            currentDisplay.CurrentState = DisplayImage.State.normal;

            foreach (var zoomInObject in zoomInObjects)
            {
                zoomInObject.gameObject.layer = 0;
            }

            Camera.main.orthographicSize = initialCameraSize;
            Camera.main.transform.position = initialCameraPosition;
        }
        else
        {
            currentDisplay.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<Sprite>("Sprites/wall" + currentDisplay.CurrentWall);
            currentDisplay.CurrentState = DisplayImage.State.normal;

            Camera.main.orthographicSize = initialCameraSize;
            Camera.main.transform.position = initialCameraPosition;

            foreach (var zoomInObject in zoomInObjects)
            {
                zoomInObject.gameObject.layer = 0;
            }
        }
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene("escape game");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickReadMessage(GameObject TextObject)
    {
        currentDisplay.CurrentState = DisplayImage.State.normal;

        Destroy(TextObject);
    }
}
