using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{


    private Animator animator;
    public GameObject minimapIndicator;
    public GameObject deliveryIndicator;
    public SinglePlayerModeMain brain;

    public Sprite texture1;
    public Sprite texture2;
    public Sprite texture3;
    public Sprite texture4;
    public Sprite texture5;
    public Sprite texture6;
    public Sprite texture7;

    Sprite[] sprites;


    private bool isDeliveryPoint = false;
    private int deliveryPointIndex = 0;
    private bool isDoorClosed = true;

    public int GetDeliveryPointIndex { get => deliveryPointIndex; set => deliveryPointIndex = value; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();


        sprites = new Sprite[] { texture1, texture2, texture3, texture4, texture5, texture6, texture7 };

        int rnd = Random.Range(0, sprites.Length + 1);


        if (rnd < sprites.Length)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[rnd];
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {


        if (col.gameObject.tag == "Player" && isDeliveryPoint)
        {
            animator.Play("open_door");
            isDoorClosed = false;

            SetDelivered();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player" && !isDoorClosed)
        {
            animator.Play("close_door");
            isDoorClosed = true;
        }
    }

    public void SetAsDeliveryPoint(int indx, SinglePlayerModeMain _brain)
    {
        brain = _brain;

        isDeliveryPoint = true;
        deliveryPointIndex = indx;
        minimapIndicator.SetActive(true);
        deliveryIndicator.SetActive(true);
    }


    public void SetDelivered()
    {
        isDeliveryPoint = false;
        minimapIndicator.SetActive(false);
        deliveryIndicator.GetComponent<Animator>().Play("deliveryIndicatorDelivered"); ;
        brain.SetDelivered(deliveryPointIndex);
    }


}
