using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPhase : Phase {
	bool _didRefresh = false;
	Animator _turnLogoAnimator;		//ターン開始時に流れるUIのAnimator

	public StartPhase( Participant turnPlayer ) {
		_turnPlayer = turnPlayer;

		GameObject turnLogo = GameObject.Find ( "TurnLogo" );
		//ターンロゴSpriteの更新----------------------------------------------------
		Image turnLogoImage = turnLogo.GetComponent<Image> ( );
		if ( _turnPlayer.gameObject.tag == "Player1" ) {
			turnLogoImage.sprite = Resources.Load<Sprite> ( "UI/ui_your_turn" );
		} else {
			turnLogoImage.sprite = Resources.Load<Sprite> ( "UI/ui_enemy_turn" );
		}
		//--------------------------------------------------------------------------
		_turnLogoAnimator = turnLogo.GetComponent<Animator>();

		_turnLogoAnimator.SetTrigger ( "cutinTrigger" );

		Debug.Log( _turnPlayer.gameObject.tag + "スタートフェーズ" );	
	}

	public override void PhaseUpdate( ) {
		if ( _didRefresh ) return;

		_turnPlayer.Refresh( );
		_turnPlayer.CardRefresh( );

		int baseLayerIndex = _turnLogoAnimator.GetLayerIndex ("Base Layer");
		AnimatorStateInfo stateInfo = _turnLogoAnimator.GetCurrentAnimatorStateInfo ( baseLayerIndex );
		if ( stateInfo.IsName( "cutin" ) && stateInfo.normalizedTime >= 1.0f ) {//カットインが終了したら
			_turnLogoAnimator.SetTrigger ( "returnIdleTrigger" );
			_didRefresh = true;
		}
	}


	public override bool IsNextPhaseFlag( ) { 
		return _didRefresh;
	}

}

//スタートフェーズ最初の一回の処理を考える
