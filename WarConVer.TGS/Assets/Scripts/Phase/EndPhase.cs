using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase : Phase {
	bool _hand = false;

	RayShooter _rayShooter = new RayShooter( );
	MainSceneOperation _mainSceneOperation = new MainSceneOperation( );

	public EndPhase( Participant turnPlayer ) {
		_turnPlayer = turnPlayer;

		if ( _turnPlayer.getHnadNum( ) <= _turnPlayer.getMaxHnadNum( ) ) { 
			_hand = true;
		}

		Debug.Log( "エンドフェーズ" );	
	}

	public override void PhaseUpdate( ) {
		if ( _hand ) return;

		if ( _mainSceneOperation.MouseTouch( ) ) {
			CardMain card = _rayShooter.RayCastHandCard( );
			if ( card == null ) return;
			_turnPlayer.HandThrowAway( card );
			if ( _turnPlayer.getHnadNum( ) == _turnPlayer.getMaxHnadNum( ) ) {
				_hand = true;
			}
		}
	}

	public override bool IsNextPhaseFlag( ) { 
		return _hand;	
	}
}

//ハンドを一枚捨てる関数を作るしかない。多分墓地も増やす
