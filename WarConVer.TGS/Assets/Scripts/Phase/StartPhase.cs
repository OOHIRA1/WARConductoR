using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPhase : Phase {

	public StartPhase( Participant turnPlayer ) {
		Debug.Log( "スタートフェーズ" );	
	}

	public override void PhaseUpdate( ) {
	}
}
