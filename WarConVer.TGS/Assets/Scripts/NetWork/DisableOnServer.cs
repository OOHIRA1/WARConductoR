using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisableOnServer : NetworkBehaviour {

	[ SerializeField ] Behaviour[ ] _behaviours;

	void Start( ) {
		if ( isServer ) { 
			foreach( var behaviour in _behaviours ) { 
				behaviour.enabled = false;	
			}	
		}
	}
	
	
}
