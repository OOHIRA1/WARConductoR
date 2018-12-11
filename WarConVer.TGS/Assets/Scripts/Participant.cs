using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour {
	[ SerializeField ] Field _field				 = null;
	[ SerializeField ] GameObject _card_in_field = null;
	[ SerializeField ] Point _active_point		 = null;
	[ SerializeField ] Point _life_point		 = null;

	void Start( ) {
		
	}
	
	
	void Update( ) {
		if ( Input.GetKeyDown( KeyCode.A ) ) APAndLPTest( );
	}

	void APAndLPTest( ) { 
		_active_point.DecreasePoint( 2 );
		_life_point.DecreasePoint( 1 );
	}

	//カードのを移動させる
	public void MoveCard( GameObject card, Square now_square, Field.DIRECTION[ ] directions, int distans, Square touch_square ) {

		List< Square > squares = new List< Square >( );

		for ( int i = 0; i < directions.Length; i++ ) {
			squares.Add( _field.SquareInThatDirection( now_square, directions[ i ], distans ) );
		}
		

		_field.ShowRange( squares, true );		//マスを赤くする(タイミングはテキトー)

		for ( int i = 0; i < squares.Count; i++ ) { 
			if ( squares[ i ].Index == touch_square.Index ) { 
				card.transform.position = touch_square.gameObject.transform.position;
				return;
			}
		}

	}
}
