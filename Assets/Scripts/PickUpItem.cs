using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour, IInteractable {

    public string DisplaySprite;
    public enum property { usable, displayable};

    public string DisplayImage;

    public string CombinationItem;

    public property itemProperty;

    private GameObject InventorySlots;

    public void Interact(DisplayImage currentDisplay)
    {
        ItemPickUp();
    }

    void Start()
    {
        
    }

    public void ItemPickUp()
    {
        InventorySlots = GameObject.Find("Slots");

        foreach (Transform slot in InventorySlots.transform)
        {
            if(slot.transform.GetChild(0).GetComponent<Image>().sprite.name == "empty_item")
            {
                slot.transform.GetChild(0).GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("Inventory Items/" + DisplaySprite);
                slot.GetComponent<Slot>().AssignProperty((int)itemProperty, DisplayImage, CombinationItem);
                Destroy(gameObject);
                break;
            }
        }
    }


}
