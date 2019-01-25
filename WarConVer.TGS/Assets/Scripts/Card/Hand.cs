using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	[ SerializeField ] int _maxHandNum = 0;
	[ SerializeField ] float _sortShiftPos = 0;
	[ SerializeField ] List< CardMain > _card  = new List< CardMain >( );
	
	string player = null;

	public int Hnad_Num { 
		get { return _card.Count; }
	}

	public int Max_Hnad_Num { 
		get { return _maxHandNum; }	
	}

	public List< CardMain > Card { 
		get	{ return _card; }
	}


	void Start( ) {
		player = this.gameObject.tag;

		Sort( player );
	}

	//手札を並べる------------------------------------------------------
	void Sort( string player ) {
		Vector3 cardPos = transform.position;
		for ( int i = 0; i < _card.Count; i++ ) {
			_card[ i ].gameObject.transform.position = cardPos;

			if ( player == ConstantStorehouse.TAG_PLAYER1 ) {
				cardPos.x += _sortShiftPos;
			} else { 
				cardPos.x -= _sortShiftPos;
			}
		}
		
	}
	//-----------------------------------------------------------------


	//手札を消費する-------------------------------------------------------------
	public void DecreaseHandCard( CardMain card ) {	//今のやりかたでは別にCardを返す理由が思いつかなかったのでvoidに変更
		for ( int i = 0; i < _card.Count; i++ ) { 
			if ( _card[ i ] != card ) continue;

			Destroy( _card[ i ].gameObject );
			_card.Remove( _card[ i ] );

			Sort( player );
			return;
		}

		Debug.Log( "[エラー]手札に登録されていないカードを使おうとしています" );
	}
	//---------------------------------------------------------------------------

	
	//手札を増やす--------------------------------
	public void IncreaseHand( CardMain card ) {
		_card.Add( card );
		card.transform.parent = this.transform;

		Sort( player );
	}
	//-------------------------------------------
}
