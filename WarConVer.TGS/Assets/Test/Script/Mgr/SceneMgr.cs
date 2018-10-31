using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class SceneMgr : MonoBehaviour {

    public enum GameFeise
    {
        Start,
        Main,
        Enemy,
        Resolut,
        MAX_SCENE_NUM
    }
    public GameFeise GetFeise;

    public void TitleSceane ( )
    {
        SceneManager.LoadScene(1);
    }
    public void MainScene()
    {
        SceneManager.LoadScene(0);
    }

}
