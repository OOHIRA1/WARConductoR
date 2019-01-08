﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase : Phase {
	bool _didHandThrowAway = false;

	RayShooter _rayShooter = new RayShooter( );
	MainSceneOperation _mainSceneOperation = new MainSceneOperation( );

	Camera _camera = null;

	public EndPhase( Participant turnPlayer, Camera turnPlayerCamera ) {
		_turnPlayer = turnPlayer;

		if ( _turnPlayer.Hand_Num <= _turnPlayer.Max_Hnad_Num ) { 
			_didHandThrowAway = true;
		}

		_camera = turnPlayerCamera;

		Debug.Log( _turnPlayer.gameObject.tag + "エンドフェーズ" );	
	}


	public override void PhaseUpdate( ) {
		if ( _didHandThrowAway ) return;

		if ( _mainSceneOperation.MouseTouch( ) ) {
			CardMain card = _rayShooter.RayCastHandCard( _turnPlayer.gameObject.tag, _camera );
			if ( card == null ) return;

			_turnPlayer.HandThrowAway( card );
			if ( _turnPlayer.Hand_Num == _turnPlayer.Max_Hnad_Num ) {
				_didHandThrowAway = true;
			}
		}
	}

	public override bool IsNextPhaseFlag( ) { 
		return _didHandThrowAway;	
	}
}

//ハンドを一枚捨てる関数を作るしかない。多分墓地も増やす
