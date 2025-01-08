using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IDragHandler, IDropHandler
{
    public int correctBoxIndex;
    public int indexOfBox { get; private set; }
   

    private Vector3 initialPosition;

   void Start()
    {
        indexOfBox = -1;
        initialPosition = transform.position;
        this.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameObject.Find("Scale").GetComponent<Scale>().isSolved) return;

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }



    public void OnDrop(PointerEventData eventData)
    {
        var scale = GameObject.Find("Scale");

        bool dropedInsideOfBox = false;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < scale.GetComponent<Scale>().ScaleBoxes.Length; i++)
        {
            if(mousePosition.x <= (scale.GetComponent<Scale>().ScaleBoxes[i].transform.position.x + 
                scale.GetComponent<Scale>().ScaleBoxes[i].GetComponent<BoxCollider2D>().size.x / 2) &&
                mousePosition.x >= (scale.GetComponent<Scale>().ScaleBoxes[i].transform.position.x -
                scale.GetComponent<Scale>().ScaleBoxes[i].GetComponent<BoxCollider2D>().size.x / 2) &&
                mousePosition.y <= (scale.GetComponent<Scale>().ScaleBoxes[i].transform.position.y +
                scale.GetComponent<Scale>().ScaleBoxes[i].GetComponent<BoxCollider2D>().size.y / 2) &&
                mousePosition.y >= (scale.GetComponent<Scale>().ScaleBoxes[i].transform.position.y -
                scale.GetComponent<Scale>().ScaleBoxes[i].GetComponent<BoxCollider2D>().size.y / 2)
                )
            {
                transform.position = new Vector3(scale.GetComponent<Scale>().ScaleBoxes[i].transform.position.x,
                    scale.GetComponent<Scale>().ScaleBoxes[i].transform.position.y, transform.position.z);

                indexOfBox = i;
                dropedInsideOfBox = true;
            }
        }

        if (!dropedInsideOfBox)
        {
            transform.position = initialPosition;
            indexOfBox = -1;
        }
    }

   
}
