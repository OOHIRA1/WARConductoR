using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPhase : Phase {
	bool _didRefresh = false;
	public StartPhase( Participant turnPlayer ) {
		_turnPlayer = turnPlayer;

		Debug.Log( _turnPlayer.gameObject.tag + "スタートフェーズ" );	
	}

	public override void PhaseUpdate( ) {
		if ( _didRefresh ) return;

		_turnPlayer.Refresh( );
		_turnPlayer.CardRefresh( );
		_didRefresh = true;
	}


	public override bool IsNextPhaseFlag( ) { 
		return _didRefresh;
	}

}

//スタートフェーズ最初の一回の処理を考える
