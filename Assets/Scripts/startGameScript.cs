using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startGameScript : MonoBehaviour
{
    public GameObject gameManager;
    private GameManager myManager;
    private Button button;
    public CanvasGroup startMenuGroup;
    private float startSaturation;

    // Use this for initialization
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        myManager = gameManager.GetComponent<GameManager>();
        //colorChanger.SetActive(false);
        //colorChangerScript.luminance = 0.0f;
        button.onClick.AddListener(TaskOnClick);
        //startSaturation = colorChangerScript.saturation;
        myManager.gameStarted = false;
        //colorChangerScript.saturation = 0.0f;
    }

    void TaskOnClick()
    {
        startMenuGroup.interactable = false;
        startMenuGroup.alpha = 0f;
        myManager.gameStarted = true;

        //StartCoroutine(setColorChangerActive());
    }

    //private IEnumerator setColorChangerActive()
    //{
    //myManager.gameStarted = true;


    /*
    yield return null;
    Debug.Log(Time.deltaTime);
    while (startMenuGroup.alpha > 0.0f){
        startMenuGroup.alpha -= Time.deltaTime;
        colorChangerScript.saturation += Time.deltaTime;
        //Debug.Log(startMenuGroup.alpha);
        if(colorChangerScript.saturation > startSaturation){
            Debug.Log("DONE");
            colorChangerScript.saturation = startSaturation;
            break;
        }
    }
    colorChangerScript.saturation = startSaturation;
    */


    //startMenuGroup.interactable = false;
    //startMenuGroup.alpha = 0f;
    //yield return null;
    //}
}
