using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exitButtonScript : MonoBehaviour {
    
    private Button button;

    // Use this for initialization
    void Start(){
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        StartCoroutine(exitGame());
    }

    private IEnumerator exitGame(){
        Application.Quit();
        yield return null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
