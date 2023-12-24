using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction_GameManager : MonoBehaviour
{
    public IntroductableItem item1;
    public IntroductableItem item2;
    public IntroductableItem item3;
    public AudioSource nextVoiceline;
    public GameObject restartButton;
    public AudioSource restartVoiceline;
    public GameObject finishButton;
    public AudioSource finishVoiceline;

    private bool isItem1Complete = false;
    private bool isItem2Complete = false;
    private bool isItem3Complete = false;
    private bool isNextVoicelineTimerStarted = false;
    private bool isOutroRan = false;
    private bool isFinishVoicelineStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BootUp());
    }

    // Update is called once per frame
    void Update()
    {
        if (item1.waitingForTouch)
        {
            StartNextVoicelineTimer(item1);
        }
        else if (item2.waitingForTouch)
        {
            StartNextVoicelineTimer(item2);
        }
        else if (item3.waitingForTouch)
        {
            StartNextVoicelineTimer(item3);
        }

        if (!isItem1Complete && item1.isComplete)
        {
            isItem1Complete = true;
            // Start item2 introducing
            item2.GetComponent<Renderer>().enabled = true;
            item2.GetComponent<Animator>().enabled = true;
        }
        else if (!isItem2Complete && item2.isComplete)
        {
            isItem2Complete = true;
            // Start item3 introducing
            item3.GetComponent<Renderer>().enabled = true;
            item3.GetComponent<Animator>().enabled = true;
        }
        else if (!isItem3Complete && item3.isComplete)
        {
            isItem3Complete = true;
        }

        // Outro
        if (!isOutroRan && isItem1Complete && isItem2Complete && isItem3Complete)
        {
            restartButton.GetComponent<Animator>().enabled = true;
            finishButton.GetComponent<Animator>().enabled = true;
            finishVoiceline.Play();
            isOutroRan = true;
            isFinishVoicelineStarted = true;
        }

        if (isFinishVoicelineStarted && !finishVoiceline.isPlaying)
        {
            restartVoiceline.Play();
            isFinishVoicelineStarted = false;
        }
    }

    void StartNextVoicelineTimer(IntroductableItem item)
    {
        if (!isNextVoicelineTimerStarted)
        {
            StartCoroutine(NextVoicelineTimer(item));
        }
    }

    IEnumerator NextVoicelineTimer(IntroductableItem item)
    {
        isNextVoicelineTimerStarted = true;
        yield return new WaitForSeconds(5);
        if (item.waitingForTouch)
        {
            nextVoiceline.Play();
        }
        isNextVoicelineTimerStarted = false;
    }

    IEnumerator BootUp()
    {
        yield return new WaitForSeconds(2);
        item1.GetComponent<Renderer>().enabled = true;
        item1.GetComponent<Animator>().enabled = true;
    }
}
