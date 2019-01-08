﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Participant : MonoBehaviour {
	const int MAX_MAGIC_POINT = 12;

	[ SerializeField ] Hand  _hand			= null;
	[ SerializeField ] Deck  _deck			= null;
	[ SerializeField ] Point _activePoint	= null;
	[ SerializeField ] Point _magicPoint    = null;
	[ SerializeField ] Point _lifePoint	    = null;
	[ SerializeField ] Point _cemetaryPoint = null;
	[ SerializeField ] Field _field			= null;

	const int SQUARE_ROW_NUM = 4;
	const int FIRST_ROW_INDEX = 0;
	const int FIFTH_ROW_INDEX = 4;
	const int ADD_CEMETARY_POINT = 1;
	const int UP_MAGIC_POINT = 1;

	List< CardMain > _cardInField  = new List< CardMain >( );			//フィールドの自分のカードの参照
	CardDamageManager _cardDamageManager = new CardDamageManager( );
	bool _loseFlag = false;

	//テスト用
	[ SerializeField ] GameObject _fieldCard = null;


	public int Hand_Num { 
		get{ return _hand.Hnad_Num; }
	}

	public int Max_Hnad_Num { 
		get { return _hand.Max_Hnad_Num; }
	}

	public int Lefe_Point { 
		get { return _lifePoint.Point_Num; }
	}

	public int Cemetary_Point { 
		get { return _cemetaryPoint.Point_Num; }	
	}

	public bool Lose_Flag { 
		get { return _loseFlag; }
		set { _loseFlag = value; }
	}


	void Start( ) {
		ReferenceCheck( );
	}

	void Update( ) {
		MyFieldCardsDeathCheck( );
	}


	//カードを移動させる-----------------------------------------------------------------------------------------------------------------------------
	public void MoveCard( CardMain card, Square nowSquare, Square moveSquare ) {
		List< Square > squares = new List< Square >( );

		//移動できるマスだけ格納
		squares = MovePossibleSquare( card, nowSquare );

		CardDamageManager.BATTLE_RESULT result = CardDamageManager.BATTLE_RESULT.NOT_BATTLE;
		//移動できるマスの中に移動したいマスがあるか探す
		for ( int i = 0; i < squares.Count; i++ ) { 
			if ( squares[ i ].Index != moveSquare.Index ) continue;

			if ( squares[ i ].On_Card != null ) {
				if ( squares[ i ].On_Card.gameObject.tag != card.gameObject.tag ) {		//移動したマスに自分のじゃないカードがあったら
					result = _cardDamageManager.CardBattleDamage( nowSquare, squares[ i ] );
					Debug.Log( result );
				}
			}

			//戦闘の結果によって移動処理を変える
			switch( result ) { 
				case CardDamageManager.BATTLE_RESULT.BOTH_DEATH:
				case CardDamageManager.BATTLE_RESULT.PLAYER_LOSE:
				case CardDamageManager.BATTLE_RESULT.BOTH_ALIVE:
					break;

				case CardDamageManager.BATTLE_RESULT.NOT_BATTLE:
				case CardDamageManager.BATTLE_RESULT.PLAYER_WIN:
					nowSquare.On_Card = null;
					moveSquare.On_Card = card;
					card.gameObject.transform.position = moveSquare.transform.position;
					break;

				default:
					Debug.Log( "予期せぬ勝敗が起きている" );
					return;
			}

			card._cardDates.actionCount++;
			_activePoint.DecreasePoint( card._cardDates.move_ap );
			return;
			
		}
		
	}
	//----------------------------------------------------------------------------------------------------------------------------------------------

	
	//ダイレクトアタック処理--------------------------------------------------
	public void DirectAttack( Participant opponentPlayer, int moveAp ) {
		_activePoint.DecreasePoint( moveAp );
		opponentPlayer._lifePoint.DecreasePoint( 1 );
	}
	//-----------------------------------------------------------------------


	//アクティブポイントが足りてるかどうかを判定する-----------
	public bool  DecreaseActivePointConfirmation( int point ) { 
		return _activePoint.DecreasePointConfirmation( point );
	}
	//---------------------------------------------------------

	
	//MPが足りてるかどうかを判定する--------------------------
	public bool DecreaseMPointConfirmation( int point ) { 
		return _magicPoint.DecreasePointConfirmation( point );	
	}
	//---------------------------------------------------------

	
	//指定のマスの色を変える---------------------------------------------------------
	public void SquareChangeColor( List< Square > squares, bool changeRed ) {
		_field.ShowRange( squares, changeRed );	//指定できるマスの色を変える
	}
	//-------------------------------------------------------------------------------


	//攻撃効果(オーバロード)---------------------------------------------------------------------
	public void UseEffect( CardMain card, Square nowSquare ) {
		List< Square > squares = new List< Square >( );
	
		//効果範囲内でカードがあるマスだけ格納
		squares = AttackEffectPossibleOnCardSquare( card, nowSquare );
		
	
		for ( int i = 0; i < squares.Count; i++ ) { 
			_cardDamageManager.CardEffectDamage( squares[ i ], card._cardDates.effect_damage );
		}

		_activePoint.DecreasePoint( card._cardDates.effect_ap );
	}
	//-------------------------------------------------------------------------------------------


	//移動効果(オーバロード)-------------------------------------------------------------
	public void UseEffect( CardMain card, Square nowSquare, Square touchSquare ) { 
		MoveCard( card, nowSquare, touchSquare );
	}
	//-----------------------------------------------------------------------------------

	
	//回復効果(オーバロード)----------------------------------------
	public void UseEffect( CardMain card ) {
		card._cardDates.hp += card._cardDates.effect_recovery_point;
		_activePoint.DecreasePoint( card._cardDates.effect_ap );
	}
	//--------------------------------------------------------------


	//移動できる場所を事前に調べる関数-------------------------------------------------------------------------------------------------
	public List< Square > MovePossibleSquare( CardMain card, Square nowSquare ) { 
		List< Square > squares = new List< Square >( );

		//移動できるマスだけ格納
		Field.DIRECTION[ ] directions = card.getDirections( card.gameObject.tag, card._cardDates.directions );
		for ( int i = 0; i < directions.Length; i++ ) {
			Square square = _field.SquareInThatDirection( nowSquare, directions[ i ], card._cardDates.distance );

			if ( square == null ) continue;
			if ( square.On_Card != null ) {
				if ( square.On_Card.gameObject.tag == card.gameObject.tag ) continue;	//マスにあるのが自分のカードだったらcontinue
			}

			squares.Add( square );
				
		}

		return squares;
	}
	//----------------------------------------------------------------------------------------------------------------------------------


	//攻撃効果をするマスにカードがあるマスを事前に調べる関数-----------------------------------------------------------------------------------
	public List< Square > AttackEffectPossibleOnCardSquare( CardMain card, Square nowSquare ) { 
		List< Square > squares = new List< Square >( );

		//攻撃できるマスだけ格納
		Field.DIRECTION[ ] directions = card.getDirections( card.gameObject.tag, card._cardDates.effect_directions );
		for ( int i = 0; i < directions.Length; i++ ) {
			Square square = _field.SquareInThatDirection( nowSquare, directions[ i ], card._cardDates.effect_ditance );

			if ( square == null ) continue;
			if ( square.On_Card == null ) continue;
			squares.Add( square );
				
		}

		Debug.Log( squares );
		return squares;
	}
	//------------------------------------------------------------------------------------------------------------------------------------

	//召喚できるマスを事前に調べる関数-----------------------------
	public List< Square > SummonSquare( string player ) { 
		List< Square > squares = new List< Square >( );
		
		if ( player == "Player1" ) { 
			for ( int i = 0; i < _field.Max_Index; i++ ) { 
				Square square = _field.getSquare( i + 1 );

				if ( ( square.Index - 1 ) / SQUARE_ROW_NUM != FIFTH_ROW_INDEX )  continue;
				if ( square.On_Card != null ) continue;

				squares.Add( square );
			}		
		}
		
		if ( player == "Player2" ) { 
			for ( int i = 0; i < _field.Max_Index; i++ ) { 
				Square square = _field.getSquare( i + 1 );

				if ( ( square.Index - 1 ) / SQUARE_ROW_NUM != FIRST_ROW_INDEX )  continue;
				if ( square.On_Card != null ) continue;

				squares.Add( square );
			}		
		}

		return squares;
	}
	//--------------------------------------------------------------

	
	//召喚処理-----------------------------------------------------------------------------------------------------------
	public void Summon( CardMain card, Square square, string player ) {
		List< Square > squares = new List< Square >( );
		squares = SummonSquare( player );

		for ( int i = 0; i < squares.Count; i++ ) { 
			if ( square.Index != squares[ i ].Index ) continue;

			_hand.DecreaseHandCard( card );
			GameObject fieldCardObj = Instantiate( _fieldCard, square.transform.position, Quaternion.identity );	//生成はHnadがやるプレイヤーがやる？
			
			CardMain fieldCard = fieldCardObj.GetComponent< CardMain >( );
			fieldCard._cardDates = card._cardDates;

			SpriteRenderer fieldCardSprite = fieldCardObj.GetComponent< SpriteRenderer >( );
			SpriteRenderer sprite = card.gameObject.GetComponent< SpriteRenderer >( );
			fieldCardSprite.sprite = sprite.sprite;

			_magicPoint.DecreasePoint( card._cardDates.mp );
			square.On_Card = fieldCard;
			AddMyFieldCards( fieldCard );
			return;
		}
	}
	//---------------------------------------------------------------------------------------------------------------------


	//手札を捨てる-----------------------------------------
	public void HandThrowAway( CardMain card ) { 
		_hand.DecreaseHandCard( card );
		_cemetaryPoint.IncreasePoint( ADD_CEMETARY_POINT );
	}
	//-----------------------------------------------------


	//ドロー処理---------------------------
	public void Draw( /*CardMain card*/ ) {
		CardMain card = _deck.Draw ( );
		card.gameObject.tag = this.gameObject.tag;	//自身のカードであることを示すタグを付ける
		card.gameObject.layer = LayerMask.NameToLayer( "HandCard" );	//手札レイヤー層に設定する
		_hand.IncreaseHand( card );
	}
	//-------------------------------------

	
	//AP,MP回復-------------------------------------------------
	public void Refresh( ) { 
		_activePoint.IncreasePoint( _activePoint.Max_Point );

		if ( _magicPoint.Max_Point < MAX_MAGIC_POINT ) {
			_magicPoint.IncreaseMaxPoint( UP_MAGIC_POINT );
		}

		_magicPoint.IncreasePoint( _magicPoint.Max_Point );
	}
	//----------------------------------------------------------

	public void CardRefresh( ) { 
		for ( int i = 0; i < _cardInField.Count; i++ ) {
			_cardInField[ i ]._cardDates.actionCount = 0;	
		}	
	}



	void AddMyFieldCards( CardMain card ) { 
		_cardInField.Add( card );
	}

	void MyFieldCardsDeathCheck( ) {
		if ( _cardInField.Count == 0 ) return;
		
		for ( int i = 0; i < _cardInField.Count; i++ ) { 
			if ( _cardInField[ i ] != null ) continue;
			_cemetaryPoint.IncreasePoint( ADD_CEMETARY_POINT );
			_cardInField.Remove( _cardInField[ i ] );
			i = 0;	
		}
	}

	

	void ReferenceCheck( ) { 
		Assert.IsNotNull( _field, "Fieldの参照がないです" );
		Assert.IsNotNull( _activePoint, "ActivePointの参照がないです" );
		Assert.IsNotNull( _lifePoint, "LifePointの参照がないです" );
		Assert.IsNotNull( _cemetaryPoint, "CemetaryPointの参照がないです" );
	}
}


//カードが存在するか、カードのtypeが何か、事前に調べる関数は別クラスにしたほうがいいかも。ブリッジバターンとかいいかも？

//Handクラスのカードを使う関数のやり方を聞いてからSummon関数を修正すること

//毎フレームずっと自分のフィールドのカードを見てるのがどうしても気になる。少し軽減できないものか。
//そのカードを見るアルゴリズムもちょっと重い気がする。
