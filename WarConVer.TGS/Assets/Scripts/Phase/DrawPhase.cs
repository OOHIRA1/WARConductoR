using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPhase : Phase {
	bool _didDraw = false;
	CardMain _drawCard = null;

	public DrawPhase( Participant turnPlayer, CardMain drawCard ) {
		_turnPlayer = turnPlayer;
		_drawCard = drawCard;

		Debug.Log( _turnPlayer.gameObject.tag + "ドローフェーズ" );	
	}

	public override void PhaseUpdate( ) {
		if ( _didDraw ) return;
 
		LoseTerms( );
		_turnPlayer.Draw( /*_drawCard*/ );
		//Player2の場合手札のカードを裏返す処理--------------------------------
		if ( _turnPlayer.gameObject.tag == ConstantStorehouse.TAG_PLAYER2 ) {
			_turnPlayer.ReverseHandCard( true );
		}
		//---------------------------------------------------------------------
		_didDraw = true;
		
		
	}

	public override bool IsNextPhaseFlag( ) { 
		return _didDraw;	
	}

	void LoseTerms( ) { 
		//デッキが０のときにカードをドローしたら
		if ( false ) {
			//_turnPlayer.Lose_Flag = true;
		}
	}
}
