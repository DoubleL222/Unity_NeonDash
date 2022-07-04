using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }
    public GameObject startObject;
    public GameObject quitObject;
    public GameObject selectionObject;
    public GameObject mainPanel;
    public Text startText;

    private GameManager myManager;
    private bool startMenu = false;
    private bool endMenu = false;
    private bool pressed;
    private float scaleSpeed = 5f;
	// Use this for initialization
	void Start () {
        pressed = false;
        _instance = this;
        myManager = FindObjectOfType<GameManager>();
        OpenStartMenu();
    }

    void OpenStartMenu()
    {
        pressed = false;
        startMenu = true;
        mainPanel.SetActive(true);
        startText.text = "START";
    }

    public void OpenEndMenu()
    {
        if (endMenu)
            return;
        pressed = false;
        endMenu = true;
        mainPanel.SetActive(true);
        startText.text = "RESTART";
        selectionObject.transform.localScale = Vector3.one * 0.5f;
    }

	// Update is called once per frame
	void Update () {
        if (!startMenu && !endMenu)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressed = true;
        }
        if (!pressed)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            selectionObject.transform.localScale = selectionObject.transform.localScale + Vector3.one * Time.deltaTime * scaleSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CheckSelection();
        }
	}

    void CheckSelection()
    {
        if (selectionObject.transform.localScale.x < startObject.transform.localScale.x)
        {
            if (startMenu)
            {
                myManager.gameStarted = true;
                startMenu = false;
                mainPanel.SetActive(false);
            }
            else if (endMenu)
            {
                SceneManager.LoadScene(0);
            }
        }
        else if (selectionObject.transform.localScale.x > startObject.transform.localScale.x && selectionObject.transform.localScale.x < quitObject.transform.localScale.x)
        {
            Application.Quit();
        }
        else
        {
            selectionObject.transform.localScale = Vector3.one * 0.5f;
        }
    }
}
