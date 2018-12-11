using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour {
	[ SerializeField ] Field _field				 = null;
	[ SerializeField ] GameObject _card_in_field = null;
	[ SerializeField ] Point _active_point		 = null;
	[ SerializeField ] Point _life_point		 = null;

	//テスト用
	[ SerializeField ] Square _now_square = null;
	[ SerializeField ] int _distans = 0;
	[ SerializeField ] Field.DIRECTION[ ] _directions = new Field.DIRECTION[ 1 ];
	List< Square > _squares = new List< Square >( );

	void Start( ) {
		
	}
	
	
	void Update( ) {
		if ( Input.GetKeyDown( KeyCode.A ) ) APAndLPTest( );
		MoveCard( _card_in_field, _now_square );
	}

	void APAndLPTest( ) { 
		_active_point.DecreasePoint( 2 );
		_life_point.DecreasePoint( 1 );
	}

	//カードのを移動させる
	public void MoveCard( GameObject card, Square now_square ) {
		for ( int i = 0; i < _directions.Length; i++ ) {
			_squares.Add( _field.SquareInThatDirection( now_square, _directions[ i ], _distans ) );
		}

		//リストで受け取ったマスをクリックしたらそこに移動する処理
		if ( Input.GetKeyDown( KeyCode.Z ) ) { 
			if ( _squares[ 0 ] == null ) return;
			card.transform.position = _squares[ 0 ].gameObject.transform.position;
		}

	}
}
