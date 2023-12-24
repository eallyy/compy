using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuizItem : MonoBehaviour
{
    public bool isClicked = false;
    public bool isActive = true;

    public void StopAnimator()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<Image>().enabled = false;
    }

    public void Click()
    {
        isClicked = true;
        GetComponent<Animator>().SetBool("out", true);
    }
}
