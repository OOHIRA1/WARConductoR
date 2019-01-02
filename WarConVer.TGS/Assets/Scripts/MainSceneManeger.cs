using System.Collections;
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
	Participant _turnPlayer = null;		//そのターンのプレイヤー
	Participant _enemyPlayer = null;	//そのターンのプレイヤーではないほう

	//ボタン
	[ SerializeField ] GameObject _returnButton		  = null;
	[ SerializeField ] GameObject _moveButton		  = null;
	[ SerializeField ] GameObject _directAttackButton = null;
	[ SerializeField ] GameObject _effectButton		  = null;
	[ SerializeField ] GameObject _effectYesBuuton	  = null;
	[ SerializeField ] GameObject _turnEndButton      = null;

	//詳細系
	[ SerializeField ] GameObject _cardDetailsImage = null;	//生成する詳細画像のプレハブ
	[ SerializeField ] GameObject _canvas			= null;	//生成したあとに子にするため

	//テスト用
	[ SerializeField ] Square	_nowSquare    = null;//テスト用でSerializeField
	[ SerializeField ] CardMain _card		  = null;
	[ SerializeField ] CardMain _drawCard	  = null;


	private void Awake( ) {
		_phase = new StartPhase( _player1 );
	}

	void Start( ) {
		_nowSquare.On_Card = _card;
		_card.gameObject.transform.position = _nowSquare.transform.position;

		//ここは先行後攻を判別して入れ替えられるようにする。
		_turnPlayer = _player1;	
		_enemyPlayer = _player2;

		ReferenceCheck( );
	}

	
	void Update( ) {
		if ( _mainSceneOperation == null ) return;
		if ( _player1 == null ) return;
		if ( _player2 == null ) return;


		if ( _phase.IsNextPhaseFlag( ) ) {
			_phaseStatus++;

			if ( ( int )_phaseStatus > ( int )PHASE.END ) { 
				_phaseStatus = PHASE.START;
				ChangePlayer( );
			}
				ChangePhase( );
		}

		if ( _phase == null ) return;

		_phase.PhaseUpdate( );


	}

	void ChangePhase( ) {
		if ( _phase != null ) _phase = null;	
		
		switch ( _phaseStatus ) { 
			case PHASE.START:
				_phase = new StartPhase( _turnPlayer );
				break;

			case PHASE.DRAW:
				_phase = new DrawPhase( _turnPlayer, _drawCard );	//多分,Deckクラスを送るのかな？
				break;

			case PHASE.MAIN:
				_phase = new MainPhase( _turnPlayer, _enemyPlayer, _mainSceneOperation,
										_returnButton, _moveButton, _directAttackButton, _effectButton, _effectYesBuuton, _turnEndButton,
										_cardDetailsImage, _canvas,
										_nowSquare, _card, _drawCard );
				break;

			case PHASE.END:
				_phase = new EndPhase( _turnPlayer );
				break;

			default:
				Debug.Log( "フェーズが正しく動作していないです" );
				return;
			
		}
	}


	void ChangePlayer( ) {
		if ( _turnPlayer == _player1 ) { 
			_turnPlayer = _player2;
			_enemyPlayer = _player1;
		} else { 
			_turnPlayer = _player1;
			_enemyPlayer = _player2;
		}
	}

	void ReferenceCheck( ) { 
		Assert.IsNotNull( _mainSceneOperation, "[エラー]MainSceneOperationが参照を取れていない" );
		Assert.IsNotNull( _player1, "[エラー]Participant(Player1)が参照を取れていない" );
		Assert.IsNotNull( _player2, "[エラー]Participant(Player2)が参照を取れていない" );
	}
}

//MainPhaseに送るプレイヤーの参照は一つにしてターンによって送る参照を切り替えるようにしたほうがいいかも？いまそれでやってみてる