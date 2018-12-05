using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDeck : MonoBehaviour {

	[SerializeField] Deck1 deck1 = null;
	// Use this for initializationthi
	void Start () {
		
	}
	
    public void PushDeckSelect()
    {
        deck1.deck_name = this.gameObject.name;
        deck1.Deck_Select();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
