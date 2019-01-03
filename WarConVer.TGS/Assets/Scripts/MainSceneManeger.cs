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

	//テスト用(プレイヤー１を中心に考えている)
	enum AHEAD_OR_REAR { 
		AHEAD,
		REAR
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

	//テスト用
	[ SerializeField ] CardMain _drawCard	  = null;
	[ SerializeField ] AHEAD_OR_REAR _aheadOrRear = AHEAD_OR_REAR.AHEAD;

	private void Awake( ) {
		//先行後攻を判別して入れ替えられる。
		if ( _aheadOrRear == AHEAD_OR_REAR.AHEAD ) {
			_turnPlayer = _player1;	
			_enemyPlayer = _player2;
		} else { 
			_turnPlayer = _player2;
			_enemyPlayer = _player1;
		}

		_phase = new StartPhase( _turnPlayer );
	}

	void Start( ) {
		ReferenceCheck( );
	}

	
	void Update( ) {
		if ( _mainSceneOperation == null ) return;
		if ( _player1 == null ) return;
		if ( _player2 == null ) return;

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
										_drawCard );
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