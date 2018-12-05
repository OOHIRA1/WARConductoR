using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMainOohiraManager : MonoBehaviour {
	public int _cardDisplayedCount = 0;//表示されたカードの数
	public GameObject[] _cards = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			if (_cardDisplayedCount < _cards.Length) {
				_cards [_cardDisplayedCount++].SetActive (true);
			}
		}
	}
}
