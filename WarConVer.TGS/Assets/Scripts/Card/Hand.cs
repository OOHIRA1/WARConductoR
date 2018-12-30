using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	const int MAX_HAND_NUM = 7;

	[ SerializeField ] GameObject _hand_card_obj = null;
	[ SerializeField ] float _sort_shift_pos = 0;
	List< CardMain > _hand_card = new List< CardMain >( );


	void Awake( ) {
		var hand_cards = gameObject.GetComponentInChildren< Transform >( ); 
		foreach( Transform card in hand_cards ) { 
			_hand_card.Add( card.gameObject.GetComponent< CardMain >( ) );	
		}
	}

	void Start( ) {
		Sort( );
	}

	//手札を並べる------------------------------------------------------
	void Sort( ) {
		Vector3 card_pos = transform.position;
		for ( int i = 0; i < _hand_card.Count; i++ ) {
			_hand_card[ i ].gameObject.transform.position = card_pos;
			card_pos.x += _sort_shift_pos;
		}
		
	}
	//-----------------------------------------------------------------


	//手札を消費する-------------------------------------------------------------
	public void UseHandCard( CardMain card ) {	//今のやりかたでは別にCsrdを返す理由が思いつかなかったのでvoidに変更
		for ( int i = 0; i < _hand_card.Count; i++ ) { 
			if ( _hand_card[ i ]._cardDates.id == card._cardDates.id ) {
				Destroy( _hand_card[ i ].gameObject );
				_hand_card.Remove( _hand_card[ i ] );

				Sort( );
				return;
			}
		}

		Debug.Log( "[エラー]手札に登録されていないカードを使おうとしています" );
	}
	//---------------------------------------------------------------------------


	//手札を増やす(生成する)--------------------------------------------------------------------------------
	public void IncreaseHand( CardMain card ) {
		if ( _hand_card.Count == MAX_HAND_NUM ) return;

		GameObject hand_card_obj = Instantiate( _hand_card_obj, transform.position, Quaternion.identity );

		CardMain hand_card = hand_card_obj.GetComponent< CardMain >( );
		hand_card._cardDates = card._cardDates;
		_hand_card.Add( hand_card );

		SpriteRenderer hand_card_sprite = hand_card_obj.GetComponent< SpriteRenderer >( );
		SpriteRenderer sprite = card.GetComponent< SpriteRenderer >( );
		hand_card_sprite.sprite = sprite.sprite;

		hand_card_obj.transform.parent = this.transform;

		Sort( );
	}
	//------------------------------------------------------------------------------------------------------
}
