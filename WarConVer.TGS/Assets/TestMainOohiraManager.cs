using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMainOohiraManager : MonoBehaviour {
	public int _cardDisplayedCount = 0;//表示されたカードの数
	public GameObject[] _cards = null;
	public int _id = 1001;
	public Deck _deck = null;

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
		if (Input.GetKeyDown (KeyCode.B)) {
			GameObject prefab = (GameObject)Resources.Load ("Prefab/Card");
			GameObject cardObj = Instantiate (prefab, Vector3.zero, Quaternion.identity);
			CardMain card = cardObj.GetComponent<CardMain> ();
			card.loadID = _id;
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			_deck.Draw ();
		}
	}
}
