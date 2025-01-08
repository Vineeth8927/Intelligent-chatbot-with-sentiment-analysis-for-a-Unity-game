using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour {

    public GameObject[] ScaleBoxes;
    public int[] Weight = new int[5];

    public GameObject ScaleDisplayer;

    private GameObject displayImage;
    private Block[] blocks;

    public bool isSolved { get; private set; }

    void Awake()
    {
        isSolved = false;
        displayImage = GameObject.Find("displayImage");
        blocks = FindObjectsOfType<Block>();
    }

    void Update()
    {
        Display();

        if (VerifySolution() && !isSolved)
        {
            isSolved = true;

            ScaleDisplayer.GetComponent<ChangeView>().SpriteName = "scale solved";

            displayImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/scale solved");
        }
    }

    void Display()
    {
        if(displayImage.GetComponent<SpriteRenderer>().sprite.name == "scale" || displayImage.GetComponent<SpriteRenderer>().sprite.name == "scale solved")
        {
            for(int i = 0; i < blocks.Length; i++)
            {
                blocks[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].gameObject.SetActive(false);
            }
        }
    }

    bool VerifySolution()
    {
        bool solved = true;

        for(int i = 0; i < ScaleBoxes.Length; i++)
        {
            if (blocks[i].indexOfBox != blocks[i].correctBoxIndex)
                solved = false;
        }

        return solved;
    }
}
