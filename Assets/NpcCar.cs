using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCar : MonoBehaviour
{

    Rigidbody2D body;

    Vector2 direction = new Vector2(50, 0);

    public GameObject sprite;

    public Sprite[] spritesFacingRight;
    public Sprite[] spritesFacingDown;
    public Sprite[] spritesFacingUp;




    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        if (body.rotation == 180)
        {
            sprite.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Refresh()
    {
        int rnd = Random.Range(0, 3);

        if (rnd == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        if (body.rotation == 0)
        {
            sprite.GetComponent<SpriteRenderer>().sprite = spritesFacingRight[(int)Random.Range(0, spritesFacingRight.Length)];
        }

        else if (body.rotation == 90)
        {
            sprite.GetComponent<SpriteRenderer>().sprite = spritesFacingUp[(int)Random.Range(0, spritesFacingUp.Length)];
        }

        else if (body.rotation == -90)
        {
            sprite.GetComponent<SpriteRenderer>().sprite = spritesFacingDown[(int)Random.Range(0, spritesFacingDown.Length)];
        }




    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "NpcChangeDirectionDown")
        {
            direction = new Vector2(0, -50);
            body.rotation = -90;
        }
    }
}
