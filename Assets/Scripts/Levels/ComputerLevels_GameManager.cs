using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerLevels_GameManager : MonoBehaviour
{
    public GameObject Level1;
    public GameObject Level2;
    public GameObject Level3;
    public GameObject Level4;
    public GameObject Level5;
    public SwipeMenu swipeMenu;

    private int savedLevel;

    // Start is called before the first frame update
    void Start()
    {
        savedLevel = PlayerPrefs.GetInt("ComputerLevel");

        // Unlocking the opened levels
        Level1.GetComponent<Button>().interactable = true;
        if (savedLevel >= 1)
        {
            Level2.GetComponent<Button>().interactable = true;
        }
        if (savedLevel >= 2)
        {
            Level3.GetComponent<Button>().interactable = true;
        }
        if (savedLevel >= 3)
        {
            Level4.GetComponent<Button>().interactable = true;
        }
        if (savedLevel >= 4)
        {
            Level5.GetComponent<Button>().interactable = true;
        }

        swipeMenu.SetScrollPos(savedLevel * 0.25f);
    }
}
