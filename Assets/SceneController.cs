using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public AudioSource titleSong;
    public UnityEngine.UI.Slider volumeSlider;
    public UnityEngine.UI.Text walletText;

    private bool rallyOwned = false;
    private bool sportOwned = false;

    private int rallyPrice = 8000;
    private int sportPrice = 15000;



    public GameObject rallyOwnedText;
    public GameObject rallyBuyPanel;
    public GameObject rallyPriceText;

    public GameObject sportOwnedText;
    public GameObject sportBuyPanel;
    public GameObject sportPriceText;

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetInt("music", 1) == 0)
        {
            titleSong.Pause();
        }

        if (PlayerPrefs.GetFloat("volume", 1) < 1)
        {
            titleSong.volume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }

        walletText.text = PlayerPrefs.GetInt("money").ToString();


        rallyOwned = intToBool(PlayerPrefs.GetInt("rally_is_unlocked", 0));
        sportOwned = intToBool(PlayerPrefs.GetInt("sport_is_unlocked", 0));

        rallyPriceText.GetComponent<UnityEngine.UI.Text>().text = rallyPrice.ToString();
        sportPriceText.GetComponent<UnityEngine.UI.Text>().text = sportPrice.ToString();

        if (rallyOwned)
        {
            rallyOwnedText.SetActive(true);
            rallyPriceText.SetActive(false);
        }

        if (sportOwned)
        {
            sportOwnedText.SetActive(true);
            sportPriceText.SetActive(false);
        }


    }


    public void StartSinglePlayerMode(string skin)
    {
        if (skin == "rally" && !rallyOwned)
        {
            rallyBuyPanel.SetActive(true);
            return;
        }

        else if (skin == "sport" && !sportOwned)
        {
            sportBuyPanel.SetActive(true);
            return;

        }


        PlayerPrefs.SetString("skin", skin);
        SceneManager.LoadScene("SinglePlayerMode");
    }



    public void BuySkin(string skin)
    {
        if (skin == "rally" && !rallyOwned)
        {
            if (rallyPrice <= PlayerPrefs.GetInt("money", 0))
            {
                rallyOwned = true;
                PlayerPrefs.SetInt("rally_is_unlocked", 1);

                rallyBuyPanel.SetActive(false);

                int money = PlayerPrefs.GetInt("money", 0);
                money -= rallyPrice;

                PlayerPrefs.SetInt("money", money);

                rallyOwnedText.SetActive(true);
                rallyPriceText.SetActive(false);
            }
        }
        else if (skin == "sport" && !sportOwned)
        {
            if (sportPrice <= PlayerPrefs.GetInt("money", 0))
            {
                sportOwned = true;
                PlayerPrefs.SetInt("sport_is_unlocked", 1);

                sportBuyPanel.SetActive(false);

                int money = PlayerPrefs.GetInt("money", 0);
                money -= sportPrice;

                PlayerPrefs.SetInt("money", money);

                sportOwnedText.SetActive(true);
                sportPriceText.SetActive(false);
            }
        }

        walletText.text = PlayerPrefs.GetInt("money").ToString();
    }


    public void setPlayerPrefsMusicPlaying(int playing)
    {
        PlayerPrefs.SetInt("music", playing);
    }
    public void setPlayerPrefsMusicVolume(float vol)
    {
        PlayerPrefs.SetFloat("volume", vol);
    }

    public void QuitGame()
    {
        Application.Quit();
    }



    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }
}
