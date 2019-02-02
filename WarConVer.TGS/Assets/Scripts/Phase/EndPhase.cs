using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase : Phase {
	bool _didHandThrowAway = false;

	RayShooter _rayShooter = new RayShooter( );
	MainSceneOperation _mainSceneOperation = null;
	UIActiveManager _uiActiveManager = null;

	public EndPhase( Participant turnPlayer, MainSceneOperation mainSceneOperation, UIActiveManager uiActiveManager ) {
		_turnPlayer = turnPlayer;
		_mainSceneOperation = mainSceneOperation;
		_uiActiveManager = uiActiveManager;

		if ( _turnPlayer.Hand_Num <= _turnPlayer.Max_Hnad_Num ) { 
			_didHandThrowAway = true;
		} else { 
            _uiActiveManager.TextActiveChanger( true, UIActiveManager.TEXT.HAND_CARD_LIMIT );
		}

		Debug.Log( _turnPlayer.gameObject.tag + "エンドフェーズ" );	
	}


	public override void PhaseUpdate( ) {
		if ( _didHandThrowAway ) return;

		if ( _turnPlayer.gameObject.tag == ConstantStorehouse.TAG_PLAYER2 ) { 
			EnemyTurnUpdate( );
			return;
		}

		PlayerTurnUpdate( );
	}

	public override bool IsNextPhaseFlag( ) { 
		return _didHandThrowAway;	
	}


	void EnemyTurnUpdate( ) {
		List< CardMain > handCards = _turnPlayer.Hand_Cards;
		CardMain card = handCards[ 0 ];

		_turnPlayer.HandThrowAway( card );
		if ( _turnPlayer.Hand_Num == _turnPlayer.Max_Hnad_Num ) {
			_didHandThrowAway = true;
		}
	}


	void PlayerTurnUpdate( ) { 
		if ( _mainSceneOperation.MouseTouch( ) ) {
			CardMain card = _rayShooter.RayCastHandCard( _turnPlayer.gameObject.tag );
			if ( card == null ) return;

			_turnPlayer.HandThrowAway( card );
			if ( _turnPlayer.Hand_Num == _turnPlayer.Max_Hnad_Num ) {
				_didHandThrowAway = true;
				_uiActiveManager.TextActiveChanger( false, UIActiveManager.TEXT.HAND_CARD_LIMIT );
			}
		}
	}
}