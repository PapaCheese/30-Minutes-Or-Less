using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using UnityEngine;

public class SteerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public int directionHorizontal = 0;
    public int directionVertical = 0;

    public Player player;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!player.CanMove())
        {
            return;
        }
        player.isControlledByUi = true;
        if (directionHorizontal != 0)
        {
            player.Horizontal = directionHorizontal;
        }
         if (directionVertical != 0)
        {
            player.Vertical = directionVertical;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (directionHorizontal != 0)
        {
            player.Horizontal = 0;
        }

        if (directionVertical != 0)
        {
            player.Vertical = 0;
        }

        if (player.Horizontal == 0 && player.Vertical == 0)
        {
            player.isControlledByUi = false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
