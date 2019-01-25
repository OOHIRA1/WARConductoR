using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==プリペアフェーズ（準備フェーズ）クラス
//
//==使用方法：PreparePhaseの処理をしたいコード内でnewする
public class PreparePhase : Phase {
	const int MAX_FIRST_HAND_NUM = 4;	//初期手札の枚数

	Participant _enemyPlayer;
	bool _isPrepareFinished;	//プリペアフェーズ処理が終わったかどうかのフラグ


	//================================================================
	//コンストラクタ
	public PreparePhase( Participant turnPlayer, Participant enemyPlayer ) {
		_turnPlayer  = turnPlayer;
		_enemyPlayer = enemyPlayer;
		_isPrepareFinished = false;

		Debug.Log ( "プリペアフェーズ" );
	}
	//=================================================================
	//=================================================================


	//=================================================================
	//仮想関数
	public override void PhaseUpdate( ) {
		if ( _isPrepareFinished ) return;

		//初期ドロー処理----------------------------------------------
		while ( _turnPlayer.Hand_Num < MAX_FIRST_HAND_NUM ) {
			_turnPlayer.Draw ( );
		}
		while ( _enemyPlayer.Hand_Num < MAX_FIRST_HAND_NUM ) {
			_enemyPlayer.Draw ( );
		}


		if ( _turnPlayer.gameObject.tag == ConstantStorehouse.TAG_PLAYER2 ) {
			_turnPlayer.ReverseHandCard (true);//エネミーだけ手札を裏返す
		} else { 
			_enemyPlayer.ReverseHandCard (true);	
		}
		//-----------------------------------------------------------

		_isPrepareFinished = true;
	}


	public override bool IsNextPhaseFlag ( ) {
		return _isPrepareFinished;
	}
	//==================================================================
	//==================================================================
}
