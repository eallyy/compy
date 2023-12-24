using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductableItem : MonoBehaviour
{
    // This class requires 2 boolean Animator parameters as "isIdle" and "isOut".
    public AudioSource Introduction_Voiceline;
    public bool isComplete = false;
    public bool waitingForTouch = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isIdle") && !Introduction_Voiceline.isPlaying)
        {
            waitingForTouch = true;
            if (Input.touchCount > 0)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isOut", true);
            }
        }
        else
        {
            waitingForTouch = false;
        }
    }

    void StartIntroduction()
    {
        // Setting idle animation true
        animator.SetBool("isIdle", true);
        Introduction_Voiceline.Play(); // Play introduction voiceline.
    }

    void EndIntroduction()
    {
        StartCoroutine(EndInstructions());
    }

    IEnumerator EndInstructions()
    {
        // Setting out animation false
        animator.SetBool("isOut", false);
        GetComponent<Renderer>().enabled = false;
        animator.enabled = false;
        yield return new WaitForSeconds(1);
        isComplete = true;
    }
}
