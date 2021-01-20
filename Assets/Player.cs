using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D body;
    public SinglePlayerModeMain brain;

    private string skin = "default";

    internal bool isControlledByUi;
    bool finishedRun = false;
    bool startedRun = false;
    float deliveryTime = 1;
    float deliveryTimer = 0;

    float horizontal;
    float vertical;
    private Animator animator;

    public float InitialDriveSpeed = 6000f;
    float driveSpeed;


    public Transform shadow;
    public Transform shadowSprite;
    public RectTransform deliveredFx;

    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float Vertical { get => vertical; set => vertical = value; }
    public bool FinishedRun { get => finishedRun; set => finishedRun = value; }
    public bool StartedRun { get => startedRun; set => startedRun = value; }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        driveSpeed = InitialDriveSpeed;

        skin = PlayerPrefs.GetString("skin", "default");

        if (skin == "rally")
        {
            InitialDriveSpeed = 7000;
        }
        else if (skin == "sport")
        {
            InitialDriveSpeed = 8000;
        }
    }

    void Update()
    {
        if (CanMove())
        {
            if (!isControlledByUi)
            {
                horizontal = Input.GetAxisRaw("Horizontal");

                vertical = (Input.GetAxisRaw("R2") - Input.GetAxisRaw("L2")) + Input.GetAxisRaw("Vertical");
            }
        }
        else
        {
            if (deliveryTimer > 0)
            {
                deliveryTimer -= Time.deltaTime;
            }
            horizontal = 0;
            vertical = 0;
        }


        float dir = transform.rotation.eulerAngles.z;

        string new_anim = "drive_right";

        if (dir < 112.5 && dir >= 67.5)
        {
            new_anim = "drive_up";
        }

        else if (dir < 292.5 && dir >= 247.5)
        {
            new_anim = "drive_down";
        }

        else if (dir < 22.5 || dir >= 337.5)
        {
            new_anim = "drive_right";
        }

        else if (dir < 67.5 && dir >= 22.5)
        {
            new_anim = "drive_up_right";
        }

        else if (dir < 337.5 && dir >= 292.5)
        {
            new_anim = "drive_down_right";
        }

        else if (dir < 202.5 && dir >= 157.5)
        {
            new_anim = "drive_left";
        }

        else if (dir < 157.5 && dir >= 112.5)
        {
            new_anim = "drive_up_left";
        }

        else if (dir < 247.5 && dir >= 202.5)
        {
            new_anim = "drive_down_left";
        }

        if (skin != "default")
        {
            new_anim = skin + "_" + new_anim;
        }

        animator.Play(new_anim);

        shadow.transform.position = new Vector3(0, -24, 0) + transform.position;


    }

    private void FixedUpdate()
    {
        //body.velocity = new Vector2(horizontal , vertical) * runSpeed;
        Vector2 vector2;
        body.rotation += -horizontal * 5;

        float rot = body.rotation;

        shadowSprite.transform.eulerAngles = new Vector3(0, 0, rot);

        vector2 = Vector2FromAngle(rot, vertical);
        body.AddForce(vector2 * driveSpeed);
    }

    public bool CanMove()
    {
        return !finishedRun && startedRun && deliveryTimer <= 0;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Grass")
        {
            driveSpeed /= 1.5f;
        }
        if (other.gameObject.tag == "Pavement")
        {
            driveSpeed /= 2;
        }
         if (other.gameObject.tag == "PizzaPlace")
        {
            brain.playerEnteredPizzaPlace();
        }
        Debug.Log("OnTriggerEnter2D Player");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Grass" || other.gameObject.tag == "Pavement")
        {
            driveSpeed = InitialDriveSpeed;
        }
        Debug.Log("OnTriggerExit2D Player");
    }

    public Vector2 Vector2FromAngle(float a, float f)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * f;
    }

    public void SteerRight()
    {
        horizontal = 1;
    }
    public void SteerLeft()
    {
        horizontal = -1;
    }

    public void PlayDeliveredFx(int money)
    {
        deliveryTimer = deliveryTime;
        deliveredFx.GetComponent<UnityEngine.UI.Text>().text = "+" + money.ToString();
        deliveredFx.GetComponent<Animator>().Play("TipAnimation", -1, 0f);
    }
}
