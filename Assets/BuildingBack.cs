using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBack : MonoBehaviour
{
    public Sprite[] sprites;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
