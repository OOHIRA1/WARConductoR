﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MainSceneManeger : MonoBehaviour {
	enum PHASE { 
		START,
		DRAW,
		MAIN,
		END,
	}

	[ SerializeField ] MainSceneOperation _mainSceneOperation = null;
	[ SerializeField ] Participant _player1 = null;
	[ SerializeField ] Participant _player2 = null;
	
	Phase _phase = null;
	PHASE _phaseStatus = PHASE.START;

	//ボタン
	[ SerializeField ] GameObject _returnButton		= null;
	[ SerializeField ] GameObject _moveButton		= null;
	[ SerializeField ] GameObject _directAttackButton = null;
	[ SerializeField ] GameObject _effectButton		= null;
	[ SerializeField ] GameObject _effectYesBuuton	= null;

	//詳細系
	[ SerializeField ] GameObject _cardDetailsImage = null;	//生成する詳細画像のプレハブ
	[ SerializeField ] GameObject _canvas			= null;	//生成したあとに子にするため

	//テスト用
	[ SerializeField ] Square	_nowSquare    = null;//テスト用でSerializeField
	[ SerializeField ] CardMain _card		  = null;
	[ SerializeField ] CardMain _drawCard	  = null;


	void Start( ) {
		_nowSquare.On_Card = _card;
		_card.gameObject.transform.position = _nowSquare.transform.position;

		ReferenceCheck( );
	}

	
	void Update( ) {
		if ( _mainSceneOperation == null ) return;
		if ( _player1 == null ) return;
		if ( _player2 == null ) return;

		if ( Input.GetKeyDown( KeyCode.A ) ) {
			ChangePhase( );
			_phaseStatus++;
			if ( ( int )_phaseStatus > ( int )PHASE.END ) { 
				_phaseStatus = PHASE.START;	
			}
		}

		if ( _phase == null ) return;

		_phase.PhaseUpdate( );

	}

	void ChangePhase( ) {
		if ( _phase != null ) { 
			_phase = null;	
		}

		switch ( _phaseStatus ) { 
			case PHASE.START:
				_phase = new StartPhase( _player1 );
				break;

			case PHASE.DRAW:
				_phase = new DrawPhase( );
				break;

			case PHASE.MAIN:
				_phase = new MainPhase( _player1, _player2, _mainSceneOperation,
										_returnButton, _moveButton, _directAttackButton, _effectButton, _effectYesBuuton,
										_cardDetailsImage, _canvas,
										_nowSquare, _card, _drawCard );
				break;

			case PHASE.END:
				_phase = new EndPhase( );
				break;
			
		}
	}

	void ReferenceCheck( ) { 
		Assert.IsNotNull( _mainSceneOperation, "[エラー]MainSceneOperationが参照を取れていない" );
		Assert.IsNotNull( _player1, "[エラー]Participant(Player1)が参照を取れていない" );
		Assert.IsNotNull( _player2, "[エラー]Participant(Player2)が参照を取れていない" );
	}
}

//MainPhaseに送るプレイヤーの参照は一つにしてターンによって送る参照を切り替えるようにしたほうがいいかも？