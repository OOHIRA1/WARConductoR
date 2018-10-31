using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHndInstant : MonoBehaviour {

    const int MAX_DRAW_COUNT = 7;

    [SerializeField] GameObject[] Hand;

    private int DrawCount = 0;
	// Use this for initialization
	void Aweak () {
        Hand = new GameObject[8];
	}

    public void OnPushButtonDraw()
    {
        if (DrawCount > MAX_DRAW_COUNT)
        {
            DrawCount = 0;
        }
        Hand[DrawCount].SetActive(true);
        DrawCount++;
    }
    public void ReLoadPush()
    {

    }

}
