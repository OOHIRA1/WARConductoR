using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Phase {
	//protected Participant _player1;
	//protected Participant _player2;

	protected Participant _turnPlayer;

	public abstract void PhaseUpdate( );

	public abstract bool IsNextPhaseFlag( );
	
}
