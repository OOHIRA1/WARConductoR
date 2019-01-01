﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	const int MAX_HAND_NUM = 7;

	[ SerializeField ] GameObject _handCardObj = null;	//生成する手札のカードオブジェクト
	[ SerializeField ] float _sortShiftPos = 0;
	List< CardMain > _card  = new List< CardMain >( );


	void Awake( ) {
		//とりあえず今は最初から設定されている手札を取得する(あとでこの処理はいらなくなるかも)
		var handCards = gameObject.GetComponentInChildren< Transform >( ); 
		foreach( Transform card in handCards ) { 
			_card.Add( card.gameObject.GetComponent< CardMain >( ) );	
		}
	}

	void Start( ) {
		Sort( );
	}

	//手札を並べる------------------------------------------------------
	void Sort( ) {
		Vector3 cardPos = transform.position;
		for ( int i = 0; i < _card.Count; i++ ) {
			_card[ i ].gameObject.transform.position = cardPos;
			cardPos.x += _sortShiftPos;
		}
		
	}
	//-----------------------------------------------------------------


	//手札を消費する-------------------------------------------------------------
	public void UseHandCard( CardMain card ) {	//今のやりかたでは別にCsrdを返す理由が思いつかなかったのでvoidに変更
		for ( int i = 0; i < _card.Count; i++ ) { 
			if ( _card[ i ]._cardDates.id == card._cardDates.id ) {
				Destroy( _card[ i ].gameObject );
				_card.Remove( _card[ i ] );

				Sort( );
				return;
			}
		}

		Debug.Log( "[エラー]手札に登録されていないカードを使おうとしています" );
	}
	//---------------------------------------------------------------------------

	
	//手札を増やす(生成する)--------------------------------------------------------------------------------
	public void IncreaseHand( CardMain card ) {
		if ( _card.Count == MAX_HAND_NUM ) return;

		GameObject handCardObj = Instantiate( _handCardObj, transform.position, Quaternion.identity );

		CardMain handCard = handCardObj.GetComponent< CardMain >( );
		handCard._cardDates = card._cardDates;
		_card.Add( handCard );

		SpriteRenderer handCardSprite = handCardObj.GetComponent< SpriteRenderer >( );
		SpriteRenderer sprite = card.GetComponent< SpriteRenderer >( );
		handCardSprite.sprite = sprite.sprite;

		handCardObj.transform.parent = this.transform;

		Sort( );
	}
	//------------------------------------------------------------------------------------------------------
}
