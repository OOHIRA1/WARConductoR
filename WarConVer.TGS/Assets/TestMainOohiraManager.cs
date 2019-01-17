using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMainOohiraManager : MonoBehaviour {
	public int _cardDisplayedCount = 0;//表示されたカードの数
	public GameObject[] _cards = null;
	public int _id = 1001;
	public Deck _deck = null;
	public AudioClip _clip;
	public CardMain _card;
	public GameObject _tapEffect;

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
			_card = _deck.Draw ();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			if (_card)
				_card.Damage (3);
		}

		if (Input.GetKeyDown (KeyCode.P)) {//これでは音は鳴らせない(Scene上にないとダメらしい)
			AudioSource audioSource = new AudioSource ();
			audioSource.clip = _clip;
			audioSource.loop = true;
			audioSource.Play ();
		}

		if (Input.GetMouseButtonDown (0)) {
			Vector3 effectPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			effectPosition.z = -9;
			Instantiate (_tapEffect, effectPosition, Quaternion.identity);
		}
	}
}
