using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToDecide : MonoBehaviour {
	[ SerializeField ] GameObject _firstDeal = null;
	[ SerializeField ] GameObject _afterAttack = null;
	[ SerializeField ] MainSceneOperation _mainSceneOperation = null;
	SceneTransition _sceneTransition = null;

	void Awake( ) {
		_sceneTransition = GetComponent< SceneTransition >( );
	}

	void Start( ) {
		int random = Random.Range( 0, 2 );
		if ( random == 0 ) { 
			//MainSceneManeger._order = MainSceneManeger.ATTACK_FIRST_OR_SECOND.FIRST;
			_firstDeal.SetActive( true );
		} else { 
			//MainSceneManeger._order = MainSceneManeger.ATTACK_FIRST_OR_SECOND.SECOND;
			_afterAttack.SetActive( true );
		}
	}
	
	void Update( ) {
		if ( _mainSceneOperation.MouseTouch( ) ) { 
			_sceneTransition.Transition( "Main" );
		}
	}

}
//緊急処置。完全に適当に作った