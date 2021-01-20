using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SinglePlayerModeMain : MonoBehaviour
{

    private int remainingDeliveries = 3;

    public ArrowIndicator indicator;

    public Player player;

    public ParticleSystem dollarsParticleSystem;

    private List<Building> allBuildings = new List<Building>();

    public UnityEngine.UI.Text tipMoneyText;
    public UnityEngine.UI.Text gainedTipMoneyText;
    public UnityEngine.UI.Text deliveriesText;


    public GameObject victoryPanel;
    public GameObject lossPanel;

    private List<Building> buildings = new List<Building>();

    public GameObject pausedPanel;

    public AudioController audioController;


    private int runMoney = 0;

    // Start is called before the first frame update
    void Start()
    {


        tipMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString();

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
                    allBuildings[rnd].transform.position) < 6000 ||
                    Vector3.Distance(allBuildings[usedBuildings[i - 1]].transform.position,
                    allBuildings[rnd].transform.position) < 6000)
                {
                    rnd = Random.Range(0, allBuildings.Count);
                }

            }
            usedBuildings.Add(rnd);
        }


        allBuildings[usedBuildings[0]].SetAsDeliveryPoint(0, this);
        allBuildings[usedBuildings[1]].SetAsDeliveryPoint(1, this);
        allBuildings[usedBuildings[2]].SetAsDeliveryPoint(2, this);

        buildings.Add(allBuildings[usedBuildings[0]]);
        buildings.Add(allBuildings[usedBuildings[1]]);
        buildings.Add(allBuildings[usedBuildings[2]]);

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


            buildings[deliveryPointIndex] = null;

            int money = Random.Range(0, 5);

            runMoney += money;

            player.PlayDeliveredFx(money);

            money += PlayerPrefs.GetInt("money");

            PlayerPrefs.SetInt("money", money);


            tipMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString();

            audioController.PlaySound("Money");
            audioController.PlaySound("DoorBell");


            dollarsParticleSystem.Play(true);

            remainingDeliveries -= 1;

            deliveriesText.text = remainingDeliveries.ToString();

            if (remainingDeliveries <= 0)
            {
                player.FinishedRun = true;
                victoryPanel.SetActive(true);
                gainedTipMoneyText.text = "Gained " + runMoney.ToString() +  "$ From Tips";
                audioController.PlaySound("Victory");

            }

        }

    }

    public void RanOutOfTime()
    {
        player.FinishedRun = true;
        lossPanel.SetActive(true);
        audioController.PlaySound("GameEnd");
    }

    public void GoToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
