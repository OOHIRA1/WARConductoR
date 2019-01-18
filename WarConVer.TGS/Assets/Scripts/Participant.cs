using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Participant : MonoBehaviour {
	const int MAX_MAGIC_POINT = 12;
	const int SQUARE_ROW_NUM = 4;
	const int FIRST_ROW_INDEX = 0;
	const int FIFTH_ROW_INDEX = 4;
	const int ADD_CEMETARY_POINT = 1;
	const int UP_MAGIC_POINT = 1;

	[ SerializeField ] Hand  _hand			= null;
	[ SerializeField ] Deck  _deck			= null;
	[ SerializeField ] Point _activePoint	= null;
	[ SerializeField ] Point _magicPoint    = null;
	[ SerializeField ] Point _lifePoint	    = null;
	[ SerializeField ] Point _cemetaryPoint = null;
	[ SerializeField ] Field _field			= null;
	[ SerializeField ] GameObject _summonEffect = null;
	[ SerializeField ] AutoDestroyBattleSpace _battleSpacePrefab = null;


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

	public List< CardMain > Card_In_Field { 
		get { return _cardInField; }	
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
		squares = _field.MovePossibleSquare( card, nowSquare );

		CardDamageManager.BATTLE_RESULT result = CardDamageManager.BATTLE_RESULT.NOT_BATTLE;
		//移動できるマスの中に移動したいマスがあるか探す
		for ( int i = 0; i < squares.Count; i++ ) { 
			if ( squares[ i ].Index != moveSquare.Index ) continue;

			if ( squares[ i ].On_Card != null ) {
				if ( squares[ i ].On_Card.gameObject.tag != card.gameObject.tag ) {		//移動したマスに自分のじゃないカードがあったら
					Sprite movingCardSprite = new Sprite();
					movingCardSprite = nowSquare.On_Card.Card_Sprite_Renderer.sprite;
					Sprite notMoveCardSprite = new Sprite ();
					notMoveCardSprite = squares[ i ].On_Card.Card_Sprite_Renderer.sprite;

					result = _cardDamageManager.CardBattleDamage( nowSquare, squares[ i ] );

					//戦闘アニメーション処理-------------------------------------------------------------------------------------------------------------
					AutoDestroyBattleSpace battleSpace = Instantiate<AutoDestroyBattleSpace> (_battleSpacePrefab, Vector3.zero, Quaternion.identity);
					switch(result) {
					case CardDamageManager.BATTLE_RESULT.PLAYER_WIN:
						battleSpace.StartRightWinAnim ( notMoveCardSprite, movingCardSprite );
						break;
					case CardDamageManager.BATTLE_RESULT.PLAYER_LOSE:
						battleSpace.StartLeftWinAnim ( notMoveCardSprite, movingCardSprite );
						break;
					case CardDamageManager.BATTLE_RESULT.BOTH_DEATH:
						battleSpace.StartBothDeathAnim ( notMoveCardSprite, movingCardSprite );
						break;
					case CardDamageManager.BATTLE_RESULT.BOTH_ALIVE:
						battleSpace.StartBothAliveAnim ( notMoveCardSprite, movingCardSprite );
						break;
					default:
						Debug.Log ("想定外の戦闘結果になりました");
						break;
					}
					//----------------------------------------------------------------------------------------------------------------------------------

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

			card.Action_Count++;
			_activePoint.DecreasePoint( card.Card_Data._necessaryAP );
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
		squares = _field.AttackEffectPossibleOnCardSquare( card, nowSquare );
		
	
		for ( int i = 0; i < squares.Count; i++ ) { 
			_cardDamageManager.CardEffectDamage( squares[ i ], card.Card_Data._effect_value );
		}

		_activePoint.DecreasePoint( card.Card_Data._necessaryAPForEffect );
	}
	//-------------------------------------------------------------------------------------------


	//移動効果(オーバロード)-------------------------------------------------------------
	public void UseEffect( CardMain card, Square nowSquare, Square touchSquare ) { 
		MoveCard( card, nowSquare, touchSquare );
	}
	//-----------------------------------------------------------------------------------

	
	//回復効果(オーバロード)----------------------------------------
	public void UseEffect( CardMain card ) {
		card.Recovery( card.Card_Data._effect_value );
		_activePoint.DecreasePoint( card.Card_Data._necessaryAPForEffect );
	}
	//--------------------------------------------------------------

	
	//召喚処理-----------------------------------------------------------------------------------------------------------
	public void Summon( CardMain card, Square square, string player ) {
		List< Square > summonableSquares = _field.SummonSquare( player );

		for ( int i = 0; i < summonableSquares.Count; i++ ) { 
			if ( square.Index != summonableSquares[ i ].Index ) continue;

			_hand.DecreaseHandCard( card );
			GameObject fieldCardObj = Instantiate( _fieldCard, square.transform.position, Quaternion.identity );	//生成はHnadがやるプレイヤーがやる？
			
			CardMain fieldCard = fieldCardObj.GetComponent< CardMain >( );
			fieldCard.loadID = card.loadID;

			//召喚エフェクト処理
			Vector3 effectPos = fieldCardObj.transform.position;
			effectPos.z = Camera.main.transform.position.z + 1f;//カメラに近い位置に生成したいため
			Instantiate( _summonEffect, effectPos, Quaternion.identity );

			_magicPoint.DecreasePoint( card.Card_Data._necessaryMP );
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
	public void Draw( ) {
		CardMain card = _deck.Draw ( );
		if ( card ) {
			card.gameObject.tag = this.gameObject.tag;	//自身のカードであることを示すタグを付ける
			card.gameObject.layer = LayerMask.NameToLayer( "HandCard" );	//手札レイヤー層に設定する
			_hand.IncreaseHand( card );
		}
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
			_cardInField[ i ].Action_Count = 0;	
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
