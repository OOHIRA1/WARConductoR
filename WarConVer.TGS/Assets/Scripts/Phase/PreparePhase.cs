using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==プリペアフェーズ（準備フェーズ）クラス
//
//==使用方法：PreparePhaseの処理をしたいコード内でnewする
public class PreparePhase : Phase {
	

	const int MAX_FIRST_HAND_NUM = 4;	//初期手札の枚数

	Participant _enemyPlayer;
	MainSceneOperation _mainSceneOperation;
	UIActiveManager _uiActiveManager;
	bool _isDrawFinished;		//初期ドローをし終わったかどうかのフラグ
	bool _isPrepareFinished;	//プリペアフェーズ処理が終わったかどうかのフラグ


	//================================================================
	//コンストラクタ
	public PreparePhase( Participant turnPlayer, Participant enemyPlayer, MainSceneOperation mainSceneOperation, UIActiveManager uiActiveManager ) {
		_turnPlayer  = turnPlayer;
		_enemyPlayer = enemyPlayer;
		_mainSceneOperation = mainSceneOperation;
		_uiActiveManager = uiActiveManager;
		_isDrawFinished = false;
		_isPrepareFinished = false;

		Debug.Log ( "プリペアフェーズ" );
	}
	//=================================================================
	//=================================================================


	//=================================================================
	//仮想関数
	public override void PhaseUpdate( ) {
		if ( _isPrepareFinished ) return;


		if ( !_isDrawFinished ) {
			//デッキシャッフル処理----------------------------------------
			_turnPlayer.Shuffle ();
			_enemyPlayer.Shuffle ();
			//-----------------------------------------------------------

			//初期ドロー処理----------------------------------------------
			while ( _turnPlayer.Hand_Num < MAX_FIRST_HAND_NUM ) {
				_turnPlayer.Draw ( );
			}
			while ( _enemyPlayer.Hand_Num < MAX_FIRST_HAND_NUM ) {
				_enemyPlayer.Draw ( );
			}


			if ( _turnPlayer.gameObject.tag == ConstantStorehouse.TAG_PLAYER2 ) {
				_turnPlayer.ReverseHandCard( true );//エネミーだけ手札を裏返す
			} else { 
				_enemyPlayer.ReverseHandCard( true );	
			}
			//-----------------------------------------------------------

			_uiActiveManager.MulliganPanelActiveChanger( true );//マリガンパネルの表示

			_isDrawFinished = true;
		}

		//マリガンYesボタンを押したときの処理--------------------------
		if ( _mainSceneOperation.MulliganYesButtonClicked( ) ) {
			_isDrawFinished = false;
			_uiActiveManager.MulliganPanelActiveChanger( false );
			_turnPlayer.ReturnCardFromHandToDeck( );
			_enemyPlayer.ReturnCardFromHandToDeck( );
		}
		//------------------------------------------------------------

		//マリガンNoボタンを押したときの処理----------------------------
		if ( _mainSceneOperation.MulliganNoButtonClicked ( ) ) {
			_uiActiveManager.MulliganPanelActiveChanger( false );
			_isPrepareFinished = true;
		}
		//------------------------------------------------------------
	}


	public override bool IsNextPhaseFlag ( ) {
		return _isPrepareFinished;
	}
	//==================================================================
	//==================================================================
}
