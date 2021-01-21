using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SinglePlayerModeMain : MonoBehaviour
{
    private float totalTimeElapsed = 0;
    private int totalDeliviriesMade = 0;



    private float startCountdown = 3;

    public int startingDeliveriesAmount = 3;
    private int remainingDeliveries;

    private int round = 1;

    public ArrowIndicator indicator;

    public Player player;

    public ParticleSystem dollarsParticleSystem;

    private List<Building> allBuildings = new List<Building>();

    public UnityEngine.UI.Text tipMoneyText;
    public UnityEngine.UI.Text gainedTipMoneyText;
    public UnityEngine.UI.Text deliveriesText;


    public Timer timer;
    public GameObject victoryPanel;
    public GameObject lossPanel;

    public Transform pizzaPlace;

    private List<Building> buildings = new List<Building>();

    public GameObject pausedPanel;

    public AudioController audioController;


    private int runMoney = 0;

    void Start()
    {
        remainingDeliveries = startingDeliveriesAmount;

        tipMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString() + "$";

        RefreshDeliveryPoints();

    }


    void RefreshDeliveryPoints()
    {
        allBuildings.Clear();
        buildings.Clear();

        GameObject[] _allBuildings = GameObject.FindGameObjectsWithTag("Building");

        foreach (GameObject go in _allBuildings)
        {
            allBuildings.Add(go.GetComponent<Building>());
        }

        List<int> usedBuildings = new List<int>();

        for (int i = 0; i < remainingDeliveries; i++)
        {
            int rnd = Random.Range(0, allBuildings.Count);

            if (i == 1)
            {

                while (Vector3.Distance(allBuildings[usedBuildings[i - 1]].transform.position,
                    allBuildings[rnd].transform.position) < 5000)
                {
                    rnd = Random.Range(0, allBuildings.Count);
                }

            }
            if (i == 2)
            {

                while (Vector3.Distance(allBuildings[usedBuildings[i - 2]].transform.position,
                    allBuildings[rnd].transform.position) < 5000 ||
                    Vector3.Distance(allBuildings[usedBuildings[i - 1]].transform.position,
                    allBuildings[rnd].transform.position) < 5000)
                {
                    rnd = Random.Range(0, allBuildings.Count);
                }

            }
            usedBuildings.Add(rnd);
        }


        for (int i = 0; i < usedBuildings.Count; i++)
        {
            allBuildings[usedBuildings[i]].SetAsDeliveryPoint(i, this);
            buildings.Add(allBuildings[usedBuildings[i]]);


        }

        indicator.target = allBuildings[usedBuildings[0]].transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForClosestBuilding();

        if (Input.GetButtonDown("Cancel"))
        {
            setPause();
        }

        if (startCountdown > 0)
        {
            startCountdown -= Time.deltaTime;
        }
        else if (!player.StartedRun)
        {
            player.StartedRun = true;
        }

        totalTimeElapsed += Time.deltaTime;
    }

    public void setPause()
    {
        audioController.PlaySound("SelectButton");

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pausedPanel.SetActive(false);
            audioController.PlaySound("UnPause");

        }
        else
        {
            Time.timeScale = 0;
            pausedPanel.SetActive(true);
            audioController.PlaySound("Pause");

        }
    }


    private void CheckForClosestBuilding()
    {

        List<float> buildingsDists = new List<float>();

        foreach (Building b in buildings)
        {
            if (b != null)
            {
                buildingsDists.Add(Vector3.Distance(player.transform.position, b.transform.position));
            }
            else // to compensate for when itereting delivered buildings
            {
                buildingsDists.Add(0);

            }
        }

        int closest = -1;

        for (int i = 0; i < buildingsDists.Count; i++)
        {
            if (buildings[i] != null)
            {
                if (closest < 0)
                {
                    closest = (int)buildingsDists[i];
                }
                if (closest > (int)buildingsDists[i])
                {
                    closest = (int)buildingsDists[i];
                }
            }
        }

        for (int i = 0; i < buildingsDists.Count; i++)
        {
            if (buildings[i] != null && closest == (int)buildingsDists[i])
            {
                indicator.target = buildings[i].transform;
                return;
            }
        }

    }


    public void SetDelivered(int deliveryPointIndex)
    {
        if (buildings[deliveryPointIndex] != null)
        {
            if (round <= 3)
            {
                timer.timeRemaining += Random.Range(1, 8 - round);
            }
            else
            {
                timer.timeRemaining += Random.Range(1, 3);
            }

            totalDeliviriesMade += 1;

            buildings[deliveryPointIndex] = null;

            int money = Random.Range(0, 5);

            runMoney += money;

            player.PlayDeliveredFx(money);

            money += PlayerPrefs.GetInt("money");

            PlayerPrefs.SetInt("money", money);


            tipMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString() + "$";

            audioController.PlaySound("Money");
            audioController.PlaySound("DoorBell");


            dollarsParticleSystem.Play(true);

            remainingDeliveries -= 1;


            if (remainingDeliveries <= 0)
            {
                indicator.target = pizzaPlace;
            }
            deliveriesText.text = remainingDeliveries.ToString();

        }

    }


    public void playerEnteredPizzaPlace()
    {
        if (remainingDeliveries <= 0)
        {
            audioController.PlaySound("ExtraTime");

            remainingDeliveries = startingDeliveriesAmount + round;
            round += 1;
            RefreshDeliveryPoints();
            deliveriesText.text = remainingDeliveries.ToString();

        }

    }


    public void RanOutOfTime()
    {
        player.FinishedRun = true;
        victoryPanel.SetActive(true);

        int minutes = (int)(totalTimeElapsed / 60);
        int seconds = (int)(totalTimeElapsed % 60);

        string strSeconds = seconds.ToString();
        if (seconds < 10)
        {
            strSeconds = "0" + seconds.ToString();
        }


        string tte = minutes.ToString() + ":" + seconds.ToString();

        gainedTipMoneyText.text = "Run Total Time: " + tte + "\n" +
            "Total Deliveries Made: " + totalDeliviriesMade.ToString() + "\n" +
            "Gained Total Of " + runMoney.ToString() + "$ From Tips";
        audioController.PlaySound("Victory");
    }

    public void GoToTitleScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreen");
    }



    public void QuitGame()
    {
        Application.Quit();
    }


}
