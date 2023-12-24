using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Quiz_GameManager : MonoBehaviour
{
    public GameObject canvas;
    [SerializeField] public GameObject opt1Prefab;
    [SerializeField] public GameObject opt2Prefab;
    [SerializeField] public GameObject opt3Prefab;
    [SerializeField] public GameObject opt4Prefab;
    [SerializeField] public GameObject opt5Prefab;
    public GameObject finishButton;
    public GameObject restartButton;
    public AudioSource correctVoiceline;
    public AudioSource wrongVoiceline;
    public AudioSource winVoiceline;
    public AudioSource loseVoiceline;
    public GameObject correctParticle;

    private GameObject[] prefabPool;
    private List<GameObject> prefabInstances;
    private GameObject answerPrefab;
    private bool isRoundActive = false;
    private bool isFinishActive = false;
    private bool isWrongActive = false;
    private int roundCounter = 0;
    private int score = 0;
    private bool isEnd = false;
    private int mistakeCount = 2;
    private int mistakeCountTemp;

    // Start is called before the first frame update
    void Start()
    {
        prefabPool = new GameObject[] { opt1Prefab, opt2Prefab, opt3Prefab, opt4Prefab, opt5Prefab };
        mistakeCountTemp = mistakeCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (roundCounter >= 5)
        {
            if (!isEnd)
            {
                isEnd = true;
                StartCoroutine(EndGame());
            }
        }
        else
        {
            if (!isRoundActive)
            {
                StartRound();
            }
            else if (prefabInstances.Count > 0 && !isFinishActive)
            {
                // Active Round
                for (int i = 0; i < prefabInstances.Count; i++)
                {
                    //Debug.Log(i.ToString() + " - " + prefabInstances[i].GetComponent<QuizItem>().isClicked);
                    if (prefabInstances[i].GetComponent<QuizItem>().isActive && prefabInstances[i].GetComponent<QuizItem>().isClicked)
                    {
                        prefabInstances[i].GetComponent<QuizItem>().isActive = false;
                        //prefabInstances[i].GetComponent<QuizItem>().isClicked = false;
                        if (prefabInstances[i] == answerPrefab)
                        {
                            // Play correct voiceline
                            // End round
                            StartCoroutine(PlayCorrect());
                        }
                        else
                        {
                            // Play wrong voiceline
                            if (!isWrongActive)
                            {
                                StartCoroutine(PlayWrong());
                            }
                        }
                    }
                }
            }
            if (isWrongActive && !isFinishActive)
            {
                for (int i = 0; i < prefabInstances.Count; i++)
                {
                    prefabInstances[i].GetComponent<Button>().interactable = false;
                }
            }
            else if (!isFinishActive)
            {
                for (int i = 0; i < prefabInstances.Count; i++)
                {
                    prefabInstances[i].GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    IEnumerator EndGame()
    {
        for (int i = 0; i < prefabInstances.Count; i++)
        {
            prefabInstances[i].GetComponent<Animator>().SetBool("out", true);
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < prefabInstances.Count; i++)
        {
            Destroy(prefabInstances[i]);
        }
        yield return new WaitForSeconds(2);
        if (score >= 3)
        {
            // Win
            winVoiceline.Play();
            // Correct particle starts
            GameObject correctParticles = Instantiate(correctParticle, new Vector2(0f, 0f), Quaternion.identity);
            Destroy(correctParticles, correctParticles.GetComponent<ParticleSystem>().main.duration + 2f);
            // Correct particle ends
            // Spawn end level button.
            finishButton.GetComponent<Animator>().enabled = true;            
        }
        else
        {
            // Lose
            loseVoiceline.Play();
            // Spawn retry button
            restartButton.GetComponent<Animator>().enabled = true;
        }
    }

    IEnumerator PlayWrong()
    {
        isWrongActive = true;
        if (mistakeCountTemp != 0)
        {
            mistakeCountTemp--;
            wrongVoiceline.Play();
            yield return new WaitForSeconds(3);
            answerPrefab.transform.Find("Voiceline").gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            roundCounter++;
            for (int i = 0; i < prefabInstances.Count; i++)
            {
                prefabInstances[i].GetComponent<Animator>().SetBool("out", true);
            }
            yield return new WaitForSeconds(1);
            for (int i = 0; i < prefabInstances.Count; i++)
            {
                Destroy(prefabInstances[i]);
                prefabInstances.RemoveAt(i);
            }
            yield return new WaitForSeconds(2);
            isRoundActive = false;
        }
        isWrongActive = false;
    }

    IEnumerator PlayCorrect()
    {
        if (!isFinishActive)
        {
            isFinishActive = true;
            roundCounter++;
            correctVoiceline.Play();
            // Correct particle starts
            GameObject correctParticles = Instantiate(correctParticle, answerPrefab.transform.position, Quaternion.identity);
            Destroy(correctParticles, correctParticles.GetComponent<ParticleSystem>().main.duration + 2f);
            // Correct particle ends
            score++;
            for (int i = 0; i < prefabInstances.Count; i++)
            {
                prefabInstances[i].GetComponent<Animator>().SetBool("out", true);
            }
            yield return new WaitForSeconds(1);
            for (int i = 0; i < prefabInstances.Count; i++)
            {
                Destroy(prefabInstances[i]);
            }
            yield return new WaitForSeconds(2);
            isFinishActive = false;
            isRoundActive = false;
        }
    }

    void StartRound()
    {
        isRoundActive = true;
        mistakeCountTemp = mistakeCount;

        // Generating prefabs
        List<GameObject> tempPrefabPool = new();
        tempPrefabPool.AddRange(prefabPool);
        List<GameObject> selectedPrefabs = GetRandomPrefabs(tempPrefabPool);

        // Creating prefabs
        List<GameObject> createdPrefabs = CreatePrefabs(selectedPrefabs);

        // Selecting answer
        int ansIndex = Random.Range(0, selectedPrefabs.Count);
        answerPrefab = createdPrefabs[ansIndex];

        // Asking question
        StartCoroutine(AskQuestion());
    }

    IEnumerator AskQuestion()
    {
        yield return new WaitForSeconds(2);
        answerPrefab.transform.Find("Voiceline").gameObject.GetComponent<AudioSource>().Play();
    }

    List<GameObject> CreatePrefabs(List<GameObject> selectedPrefabs)
    {
        List<Vector2> positions = new List<Vector2> {
            new Vector2(-585f, 270f), // Left Upper
            new Vector2(585f, 270f), // Right Upper
            new Vector2(585f, -270f), // Right Under
            new Vector2(-585f, -270f) // Left Under
        };

        List<GameObject> createdPrefabs = new();

        while (selectedPrefabs.Count > 1)
        {
            int index = Random.Range(0, selectedPrefabs.Count);
            int posIndex = Random.Range(0, positions.Count);
            GameObject temp = Instantiate(selectedPrefabs[index], positions[posIndex], Quaternion.identity) as GameObject;
            temp.transform.SetParent(canvas.transform, false);
            createdPrefabs.Add(temp);
            selectedPrefabs.RemoveAt(index);
            positions.RemoveAt(posIndex);
        }

        prefabInstances = createdPrefabs;
        return createdPrefabs;
    }

    List<GameObject> GetRandomPrefabs(List<GameObject> prefabList)
    {
        List<GameObject> selectedPrefabs = new();
        while (prefabList.Count > 0)
        {
            int index = Random.Range(0, prefabList.Count);
            GameObject prefab = prefabList[index];

            prefabList.RemoveAt(index);
            selectedPrefabs.Add(prefab);
        }
        return selectedPrefabs;
    }
}
