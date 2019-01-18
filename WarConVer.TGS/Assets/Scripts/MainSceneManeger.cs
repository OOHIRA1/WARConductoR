﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MainSceneManeger : MonoBehaviour {
	enum PHASE { 
		PREPARE,
		START,
		DRAW,
		MAIN,
		END,
	}

	//テスト用(プレイヤー１を中心に考えている)
	enum ATTACK_FIRST_OR_SECOND { 
		FIRST,
		SECOND
	}

	[ SerializeField ] MainSceneOperation _mainSceneOperation = null;
	[ SerializeField ] Participant _player1 = null;
	[ SerializeField ] Participant _player2 = null;
	[ SerializeField ] UIActiveManager _uIActiveManager = null;
	[ SerializeField ] EnemyBehavior _enemyBehavior = null;
	[ SerializeField ] Field _field = null;

	//エフェクト系
	[ SerializeField ] GameObject _tapEffect = null;
	
	Phase _phase = null;
	PHASE _phaseStatus = PHASE.PREPARE;
	Participant _turnPlayer = null;		//そのターンのプレイヤー
	Participant _enemyPlayer = null;	//そのターンのプレイヤーではないほう

	//ボタン
	[ SerializeField ] GameObject _turnEndButton = null;

	//テスト用
	[ SerializeField ] CardMain _drawCard	  = null;
	[ SerializeField ] ATTACK_FIRST_OR_SECOND _order = ATTACK_FIRST_OR_SECOND.FIRST;
	[ SerializeField ] CardMain _debugCard = null;
	[ SerializeField ] Square _debugSquare = null;


	private void Awake( ) {
		//先行後攻を判別して入れ替えられる。
		if ( _order == ATTACK_FIRST_OR_SECOND.FIRST ) {
			_turnPlayer = _player1;	
			_enemyPlayer = _player2;
		} else { 
			_turnPlayer = _player2;
			_enemyPlayer = _player1;
		}

		_phase = new PreparePhase( _turnPlayer, _enemyPlayer );
	}

	void Start( ) {
		ReferenceCheck( );
	}

	
	void Update( ) {

		if ( _player1.Lose_Flag ) { 
			Debug.Log( "Player2の勝ちです" );
			return;
		}

		if ( _player2.Lose_Flag ) { 
			Debug.Log( "Player1の勝ちです" );
			return;
		}

		if ( _phase.IsNextPhaseFlag( ) ) {
			_phaseStatus++;

			if ( ( int )_phaseStatus > ( int )PHASE.END ) { 
				_phaseStatus = PHASE.START;
				ChangePlayer( );
			}

			ChangePhase( );
		}

		Assert.IsNotNull( _phase, "[エラー]フェーズが正常に入ってないです" );

		_phase.PhaseUpdate( );


		//タップエフェクト処理-------------------------------------------------------------------
		if( _mainSceneOperation.MouseTouch() ) {
			Vector3 effectPos = _mainSceneOperation.getWorldMousePos ();
			effectPos.z = Camera.main.transform.position.z + 1f;//カメラに近い位置に生成したいため
			Instantiate( _tapEffect, effectPos, Quaternion.identity );
		}
		//--------------------------------------------------------------------------------------
	}

	void ChangePhase( ) {
		if ( _phase != null ) _phase = null;	
		
		switch ( _phaseStatus ) { 
			case PHASE.PREPARE:
				_phase = new PreparePhase ( _turnPlayer, _enemyPlayer );
				break;

			case PHASE.START:
				_phase = new StartPhase( _turnPlayer );
				break;

			case PHASE.DRAW:
				_phase = new DrawPhase( _turnPlayer, _drawCard );	//多分,Deckクラスを送るのかな？
				break;

			case PHASE.MAIN:
				_phase = new MainPhase( _turnPlayer, _enemyPlayer, _mainSceneOperation, _uIActiveManager, _field,
									    _turnEndButton,
										_enemyBehavior,
										_drawCard, _debugCard, _debugSquare );
				break;

			case PHASE.END:
				_phase = new EndPhase( _turnPlayer, _mainSceneOperation );
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