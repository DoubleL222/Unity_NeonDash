using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Level counter
    private int level = 0;

    //Object prefabs and object queue
    public GameObject[] prefabs = new GameObject[3];
    public GameObject myParticle;
    private GameObject[] objectQueue = new GameObject[3];

    //Objects: toMatch (the one being manipulated); toBeMatch (the target object); Upcoming (next object in queue, i.e. the background)
    private GameObject toMatch;
    private GameObject toBeMatched;
    private GameObject upComing;
    private int randomPrefab;

    //Check size or rotation
    private bool checkSizeUp = false;
    private bool checkSizeDown = false;
    private bool checkRotationRight = false;
    private bool checkRotationLeft = false;
    private float randomRotation;
    public float minSizeOffset = 0.5f;
    public float maxSizeOffset = 0.5f;
    public float rotationOffset = 5.0f;

    //State control
    private bool failLevel = false;
    [HideInInspector]
    public bool gameStarted = false;

    //Rates for scaling and rotating
    public float sizeRate = 5.0f;
    public float rotationRate = 5.0f;

    //For colors
    private float saturation = 1.0f;
    private float luminance = 1.0f;
    public float offsetAngle1 = 140.0f;
    public float offsetAngle2 = 220.0f;
    private Color[] targetColors;
    private float targetAngle;
    private float previousAngle;
    private float appliedAngle = 0.0f;
    private float t_index_Color;
    public float timeToChangeColor = 0.5f;

    //Score
    public Text scoreText;
    private string finalScoreText;


    void Start()
    {
        setupGame();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Space) && gameStarted)
        {
            if (checkSizeUp)
            {
                toMatch.transform.localScale += new Vector3(Time.deltaTime * sizeRate, Time.deltaTime * sizeRate, 0);

                if ((toMatch.transform.localScale.x > toBeMatched.transform.localScale.x + 1.5) && (toMatch.transform.localScale.y > toBeMatched.transform.localScale.y + 1.5))
                {
                    checkSizeUp = false;
                    checkSizeDown = true;
                }

            }
            else if (checkSizeDown)
            {
                toMatch.transform.localScale -= new Vector3(Time.deltaTime * sizeRate, Time.deltaTime * sizeRate, 0);

                if ((toMatch.transform.localScale.x < toBeMatched.transform.localScale.x - 0.7) && (toMatch.transform.localScale.y < toBeMatched.transform.localScale.y - 0.7))
                {
                    checkSizeUp = true;
                    checkSizeDown = false;
                }
            }
            else if (checkRotationRight)
            {
                toMatch.transform.eulerAngles -= new Vector3(0, 0, Time.deltaTime * rotationRate);
            }
            else if (checkRotationLeft)
            {
                toMatch.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * rotationRate);
            }
        }


        if (Input.GetKeyUp(KeyCode.Space) && gameStarted)
        {
            checkToPassNextLevel();
        }

        if (Input.GetKeyUp(KeyCode.G) && gameStarted)
        {
            nextLevel();
        }
        if (saturation <= 0)
        {
            if (finalScoreText == null)
            {
                finalScoreText = scoreText.text;
            }
            scoreText.text = finalScoreText;
            //Set game logic to fail the game here?
            UIManager.instance.OpenEndMenu();
        }

        //Changes colors over time when new levels are achieved
        t_index_Color += Time.deltaTime / timeToChangeColor;

        changeColorOverTime();

        toMatch.GetComponent<Renderer>().material.color = new Color(targetColors[1].r, targetColors[1].g, targetColors[1].b, 0.5f);
        toBeMatched.GetComponent<Renderer>().material.color = targetColors[0];
        upComing.GetComponent<Renderer>().material.color = targetColors[2];

    }

    //START GAME
    private void setupGame()
    {
        changeCheckState();

        randomPrefab = Random.Range((int)0, (int)prefabs.Length);
        objectQueue[0] = prefabs[randomPrefab];
        objectQueue[1] = objectQueue[0];
        randomPrefab = Random.Range((int)0, (int)prefabs.Length);
        objectQueue[2] = prefabs[randomPrefab];

        toMatch = Instantiate(objectQueue[0], Vector3.zero, Quaternion.identity);
        toBeMatched = Instantiate(objectQueue[1], Vector3.zero, Quaternion.identity);
        upComing = Instantiate(objectQueue[2], Vector3.zero, Quaternion.identity);

        setSizeAndRotation();
    }

    //NEXT LEVEL
    private void nextLevel()
    {
        level++;
        scoreText.text = "" + level;
        changeCheckState();
        //sizeRate += 0.1f;
        //rotationRate += 1.0f;
        randomPrefab = Random.Range((int)0, (int)prefabs.Length);

        previousAngle = targetAngle;
        targetAngle += 30.0f;
        //if (targetAngle == 360.0f)
        //targetAngle = 0;
        t_index_Color = 0;

        //Placeholder area for some particle effect when passing a level
        //GameObject nextLevelParticle = Instantiate(myParticle, objectQueue[1].transform.position, Quaternion.identity);
        //nextLevelParticle.GetComponent<ControlParticles>().psStartSize = objectQueue[1].GetComponent<Transform>().localScale.x * 10;
        //Destroy(nextLevelParticle, 1.0f);

        //Store information before deleting if need be
        Destroy(toMatch);
        Destroy(toBeMatched);
        Destroy(upComing);

        objectQueue[0] = objectQueue[2];
        objectQueue[1] = objectQueue[2];
        objectQueue[2] = prefabs[randomPrefab];

        toMatch = Instantiate(objectQueue[0], Vector3.zero, Quaternion.identity);
        toBeMatched = Instantiate(objectQueue[1], Vector3.zero, Quaternion.identity);
        upComing = Instantiate(objectQueue[2], Vector3.zero, Quaternion.identity);

        setSizeAndRotation();
    }

    //SET SIZE OF SPRITES
    void setSizeAndRotation()
    {
        float randomMatchSize = Random.Range(1.0f, 2.0f);
        float randomMatchRotation = Random.Range(60.0f, 180f);
        randomRotation = randomMatchRotation;
        Vector3 newSize = new Vector3(randomMatchSize, randomMatchSize, 1.0f);
        Vector3 upComingSize = new Vector3(50f, 50f, 0.1f);

        if (checkSizeUp)
        {
            toMatch.transform.localScale = new Vector3(0.5f, 0.5f, 0.1f); //some random initial size
            toBeMatched.transform.localScale = newSize; //sets a new size to match
            upComing.transform.localScale = upComingSize; //sets the size of the "background" object
        }
        else if (checkSizeDown)
        {
            toMatch.transform.localScale = new Vector3(2.0f, 2.0f, 0.1f); //some random initial size
            float randomMatchSizeSmall = Random.Range(0.5f, 1.5f);
            Vector3 newSizeSmall = new Vector3(randomMatchSizeSmall, randomMatchSizeSmall, 1.0f);
            toBeMatched.transform.localScale = newSizeSmall; //sets a new size to match
            upComing.transform.localScale = upComingSize; //sets the size of the "background" object
        }
        else if (checkRotationLeft)
        {
            toMatch.transform.localScale = newSize; //some random initial size
            toBeMatched.transform.localScale = newSize; //sets a new size to match
            toBeMatched.transform.eulerAngles = new Vector3(0, 0, -randomMatchRotation);
            upComing.transform.localScale = upComingSize; //sets the size of the "background" object
        }
        else if (checkRotationRight)
        {
            toMatch.transform.localScale = newSize; //some random initial size
            toBeMatched.transform.localScale = newSize; //sets a new size to match
            toBeMatched.transform.eulerAngles = new Vector3(0, 0, randomMatchRotation);
            upComing.transform.localScale = upComingSize; //sets the size of the "background" object
        }
    }

    //GENERATE COLORS
    public static Color[] generateOffsetColors(
    float offsetAngle1,
    float offsetAngle2,
    float saturation,
    float luminance,
    float angle)
    {
        Color[] outputColors = new Color[3];

        outputColors[0] = Color.HSVToRGB(((angle) / 360.0f) % 1.0f, saturation, luminance);
        outputColors[1] = Color.HSVToRGB(((angle + offsetAngle1) / 360.0f) % 1.0f, saturation, luminance);
        outputColors[2] = Color.HSVToRGB(((angle + offsetAngle2) / 360.0f) % 1.0f, saturation, luminance);

        return outputColors;
    }

    void changeColorOverTime()
    {
        appliedAngle = Mathf.Lerp(previousAngle, targetAngle, t_index_Color);
        targetColors = generateOffsetColors(offsetAngle1, offsetAngle2, saturation, luminance, appliedAngle);
    }

    private void checkToPassNextLevel()
    {
        //Check to pass scale
        if (checkSizeUp || checkSizeDown)
        {
            if ((toMatch.transform.localScale.x >= toBeMatched.transform.localScale.x - minSizeOffset &&
                toMatch.transform.localScale.y >= toBeMatched.transform.localScale.y - minSizeOffset) &&

                (toMatch.transform.localScale.x <= toBeMatched.transform.localScale.x + maxSizeOffset &&
                toMatch.transform.localScale.y <= toBeMatched.transform.localScale.y + maxSizeOffset))
            {
                Debug.Log("Level Scale Passed!");
                nextLevel();
            }
            else
            {
                Debug.Log("Level Scale !NOT! Passed!");
                saturation -= 0.2f;
                nextLevel();
            }
        }

        //Check to pass rotation
        else if (checkRotationLeft || checkRotationRight)
        {
            int rotationVertCount = toMatch.GetComponent<VertCount>().getVertCount();
            int rotationIndex = 360 / rotationVertCount;
            bool rotationPassed = false;
            for (int i = 1; i <= rotationVertCount; i++)
            {
                float rotationChecker = toMatch.transform.eulerAngles.z + rotationIndex * i;
                if (rotationChecker > 360.0f)
                    rotationChecker -= 360.0f;

                if ((rotationChecker >= toBeMatched.transform.eulerAngles.z - rotationOffset) &&
                    (rotationChecker <= toBeMatched.transform.eulerAngles.z + rotationOffset))
                {
                    Debug.Log("Level Rotation Passed!");
                    rotationPassed = true;
                    nextLevel();
                    break;
                }
            }
            if (!rotationPassed)
            {
                Debug.Log("Level Rotation !NOT! Passed!");
                saturation -= 0.2f;
                nextLevel();
            }
        }
    }

    private void changeCheckState()
    {
        //Remove comment to engage switch
        int randomState = Random.Range((int)1, (int)5);
        //int randomState = 1;
        switch (randomState)
        {
            case 1:
                checkSizeUp = true;
                checkSizeDown = false;
                checkRotationRight = false;
                checkRotationLeft = false;
                break;
            case 2:
                checkSizeUp = false;
                checkSizeDown = true;
                checkRotationRight = false;
                checkRotationLeft = false;
                break;
            case 3:
                checkSizeUp = false;
                checkSizeDown = false;
                checkRotationRight = true;
                checkRotationLeft = false;
                break;
            case 4:
                checkSizeUp = false;
                checkSizeDown = false;
                checkRotationRight = false;
                checkRotationLeft = true;
                break;
        }
    }
}
