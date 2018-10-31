using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConDeck : MonoBehaviour {

    [SerializeField] Deck1 deck1;
	// Use this for initializationthi
	void Start () {
		
	}
	
    public void PushDeckSelect()
    {
        deck1.deck_name = this.gameObject.name;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
