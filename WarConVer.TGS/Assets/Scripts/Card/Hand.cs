using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	//const int MAX_ORVER_HAND_NUM = 1;	//手札が持てる最大数を超えて持てる枚数の最大値(_maxHn)
	//const int MAX_HAND_NUM = 7;

	[ SerializeField ] int _maxHandNum = 0;
	[ SerializeField ] GameObject _handCardObj = null;	//生成する手札のカードオブジェクト
	[ SerializeField ] float _sortShiftPos = 0;
	[ SerializeField ] List< CardMain > _card  = new List< CardMain >( );

	//ネットワーク用 (テスト)
	[ SerializeField ] GameObject _uraCard = null;
	[ SerializeField ] GameObject[ ] _a = new GameObject[ 1 ];
	[ SerializeField ] List< GameObject > _uraCards  = new List< GameObject >( );

	public int Hnad_Num { 
		get { return _card.Count; }
	}

	public int Max_Hnad_Num { 
		get { return _maxHandNum; }	
	}


	void Awake( ) {
		//とりあえず今は最初から設定されている手札を取得する(あとでこの処理はいらなくなるかも)
		//var handCards = gameObject.GetComponentInChildren< Transform >( ); 
		/*foreach( Transform card in handCards ) { 
			_card.Add( card.gameObject.GetComponent< CardMain >( ) );	
		}


		for ( int i = 0; i < _a.Length; i++ ) { 
			_uraCards.Add( _a[ i ] );
		}
		*/
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

		//ネットワークテスト用
		for ( int i = 0; i < _uraCards.Count; i++ ) { 
			_uraCards[ i ].transform.position = _card[ i ].gameObject.transform.position;	
		}
		
	}
	//-----------------------------------------------------------------


	//手札を消費する-------------------------------------------------------------
	public void DecreaseHandCard( CardMain card ) {	//今のやりかたでは別にCardを返す理由が思いつかなかったのでvoidに変更
		for ( int i = 0; i < _card.Count; i++ ) { 
			if ( _card[ i ]._cardDates.id != card._cardDates.id ) continue;

			Destroy( _card[ i ].gameObject );
			_card.Remove( _card[ i ] );

			//ネットワークテスト用
			Destroy( _uraCards[ i ] );
			_uraCards.Remove( _uraCards[ i ] );

			Sort( );
			return;
		}

		Debug.Log( "[エラー]手札に登録されていないカードを使おうとしています" );
	}
	//---------------------------------------------------------------------------

	
	//手札を増やす(生成する)--------------------------------------------------------------------------------
	public void IncreaseHand( CardMain card ) {
		//if ( _card.Count == _maxHandNum + 1 ) return;		//今のところ手札が持てる最大枚数より多くなるのは１枚までなので +1 している

		//GameObject handCardObj = Instantiate( _handCardObj, transform.position, Quaternion.identity );

		//CardMain handCard = handCardObj.GetComponent< CardMain >( );
		//handCard._cardDates = card._cardDates;
		//_card.Add( handCard );
		_card.Add( card );

		//SpriteRenderer handCardSprite = handCardObj.GetComponent< SpriteRenderer >( );
		//SpriteRenderer sprite = card.GetComponent< SpriteRenderer >( );
		//handCardSprite.sprite = sprite.sprite;

		//handCardObj.transform.parent = this.transform;
		card.transform.parent = this.transform;

		//ネットワークテスト用
		GameObject uraCardObj = Instantiate( _uraCard, transform.position, Quaternion.identity );
		_uraCards.Add( uraCardObj );

		Sort( );
	}
	//------------------------------------------------------------------------------------------------------
}
