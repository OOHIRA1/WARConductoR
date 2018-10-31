using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ttle : MonoBehaviour {

    

    public void Sceneove()
    {
        //1はtutorial　
        SceneManager.LoadScene(1);
    }
    public void SceneGame()
    {
        SceneManager.LoadScene(2);
    }
    public void Title()
    {
        SceneManager.LoadScene(0);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
