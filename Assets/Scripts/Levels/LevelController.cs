using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public void SetComputerLevel(int value)
    {
        int temp = PlayerPrefs.GetInt("ComputerLevel");
        if (value > temp)
        {
            PlayerPrefs.SetInt("ComputerLevel", value);
        }
    }

    public void SetHardwareLevel(int value)
    {
        int temp = PlayerPrefs.GetInt("HardwareLevel");
        if (value > temp)
        {
            PlayerPrefs.SetInt("HardwareLevel", value);
        }
    }
}
