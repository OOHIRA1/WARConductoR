using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//==カード機能クラス
//
//==使用方法：カードのGameObjectにアタッチ
public class CardMain : MonoBehaviour {
	const int _MAX_ACTION_COUNT = 3;							//移動回数の最大値
	const float DETAILS_POS_X = -210f;

	[SerializeField] int _loadID = 0;							//読み込むカードID
	[SerializeField] SpriteRenderer _cardSpriteRenderer = null;	//カードのSpriteRenderer
	[SerializeField] CardDataLoader _cardDataLoader = null;
	[SerializeField] CardData _cardData = null;
	[SerializeField] int _actionCount = 0;						//移動回数

	GameObject _cardDetailsImage = null;
	GameObject _details = null;
	GameObject _canvas = null;

	//テスト用----------------------------------------------
	[System.Serializable]
	public struct CardDates {
		public int id;
		public int hp;
		public int max_hp;
		public int attack_point;
		public Field.DIRECTION[ ] directions;
		public int distance;
		public int move_ap;
		public Field.DIRECTION[ ] effect_directions;
		public int effect_ap;
		public int effect_ditance;
		public int effect_damage;
		public int effect_recovery_point;
		public int mp;
		public CardData.EFFECT_TYPE effect_type;
		public int actionCount; 
		
	}
	public CardDates _cardDates = new CardDates( );
	//-------------------------------------------------------


	//===================================================================
	//アクセッサ
	public int loadID {
		get { return _loadID; }
		set { _loadID = value; }//カードをInstantiateした際にIDを設定する
	}

	public SpriteRenderer Card_Sprite_Renderer { 
		get { return _cardSpriteRenderer; }
	}

	public int MAX_ACTION_COUNT { 
		get { return _MAX_ACTION_COUNT; }	
	}

	//===================================================================
	//===================================================================


	// Use this for initialization
	void Start () {
		_cardSpriteRenderer = GetComponent<SpriteRenderer>();
		_cardDataLoader = GameObject.Find ("CardDataLoader").GetComponent<CardDataLoader>();
		Load ();//カードデータの読み込み
		Debug.Log (_cardData);
		Debug.Log (_actionCount);
		_actionCount = _MAX_ACTION_COUNT;

		_cardDetailsImage = Resources.Load< GameObject >( "Prefab/Dummy/CardDetailsImage" );
		_canvas = GameObject.Find( "Canvas" );
	}


	//--カードデータを読み込む関数
	void Load( ) {
		_cardSpriteRenderer.sprite = (Sprite)Resources.Load<Sprite> ("Card/" + _loadID);//カード画像の読み込み
		_cardData = _cardDataLoader.GetCardDataFromID ( _loadID );
	}


	//================================================b==========================================================
	//public関数

	//ダメージ-----------------------------------
	public void Damage( int decreasePoint  ) { 
		_cardDates.hp -= decreasePoint;

		if ( _cardDates.hp < 0 ) { 
			_cardDates.hp = 0;	
		}

		//HPが０になったら破壊する
		if ( _cardDates.hp == 0 ) { 
			Death( );	
		}
	}
	//------------------------------------------

	//破壊
	public void Death( ) {
		Destroy( this.gameObject );	
	}


	//プレイヤーによってカードの持っている向きを調整して返す処理------------------------------------
	public Field.DIRECTION[ ] getDirections( string player, Field.DIRECTION[ ] directions ) {
		if ( player == "Player1" ) {
			return directions;
		}

		Field.DIRECTION[ ] player2Directions = new Field.DIRECTION[ directions.Length ];
		for ( int i = 0; i < directions.Length; i++ ) { 
		
			switch( directions[ i ] ) { 
				case Field.DIRECTION.LEFT_FORWARD:
					player2Directions[ i ] = Field.DIRECTION.RIGHT_BACK;
					break;

				case Field.DIRECTION.FORWAED:
					player2Directions[ i ] = Field.DIRECTION.BACK;
					break;

				case Field.DIRECTION.RIGHT_FORWARD:
					player2Directions[ i ] = Field.DIRECTION.LEFT_BACK;
					break;

				case Field.DIRECTION.LEFT:
					player2Directions[ i ] = Field.DIRECTION.RIGHT;
					break;

				case Field.DIRECTION.RIGHT:
					player2Directions[ i ] = Field.DIRECTION.LEFT;
					break;

				case Field.DIRECTION.LEFT_BACK:
					player2Directions[ i ] = Field.DIRECTION.RIGHT_FORWARD;
					break;

				case Field.DIRECTION.BACK:
					player2Directions[ i ] = Field.DIRECTION.FORWAED;
					break;

				case Field.DIRECTION.RIGHT_BACK:
					player2Directions[ i ] = Field.DIRECTION.LEFT_FORWARD;
					break;
				
				default:
					break;
			}
		}

		return player2Directions;
	}
	//-----------------------------------------------------------------------------------------------


	public void ShowCardDetail( ) {
		//カードの詳細プレハブの取得
		_details = Object.Instantiate( _cardDetailsImage, Vector3.zero, Quaternion.identity );
		Text attackPoint = _details.transform.Find( "Attack_Point_Background/Attack_Point" ).GetComponent< Text >( );
		Text hitPoint = _details.transform.Find( "Hit_Point_Background/Hit_Point" ).GetComponent< Text >( );

		//画像などの情報読み込み
		_details.GetComponent< Image >( ).sprite = _cardSpriteRenderer.sprite;
		attackPoint.text = _cardDates.attack_point.ToString( );
		hitPoint.text = _cardDates.hp.ToString( );

		//位置をずらしている
		_details.transform.parent = _canvas.transform;
		RectTransform detailsPos = _details.GetComponent< RectTransform >( );
		detailsPos.localPosition = new Vector3( DETAILS_POS_X, 0, 0 );	//あとでこの部分の処理は修正するだろうからマジックナンバーを放置	
	}

	public void DeleteCardDetail( ) { 
		Destroy( _details );
		_details = null;
	}

	//===========================================================================================================
	//===========================================================================================================
}
