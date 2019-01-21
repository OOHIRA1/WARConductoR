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
	[SerializeField] CardData _cardData = new CardData();
	[SerializeField] int _actionCount = 0;						//移動回数

	GameObject _cardDetailsImage = null;
	GameObject _details = null;
	GameObject _canvas = null;


	//===================================================================
	//アクセッサ
	public int loadID {
		get { return _loadID; }
		set { _loadID = value; }//カードをInstantiateした際にIDを設定する
	}

	public SpriteRenderer Card_Sprite_Renderer { 
		get { return _cardSpriteRenderer; }
	}

	public CardData Card_Data {
		get { return _cardData; }
	}

	public int Action_Count {
		get { return _actionCount; }
		set { _actionCount = value; }
	}

	public int MAX_ACTION_COUNT { 
		get { return _MAX_ACTION_COUNT; }	
	}

	//テスト用
	public List< Field.DIRECTION > Card_Data_Direction {
		get { return _cardData._directionOfTravel; }
		set { _cardData._directionOfTravel = value; }
	}

	//===================================================================
	//===================================================================


	// Use this for initialization
	void Start () {
		_cardSpriteRenderer = GetComponent<SpriteRenderer>();
		_cardDataLoader = GameObject.Find ("CardDataLoader").GetComponent<CardDataLoader>();
		Load ();//カードデータの読み込み
		_actionCount = 0;

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
		_cardData._toughness -= decreasePoint;

		if ( _cardData._toughness < 0 ) { 
			_cardData._toughness = 0;	
		}

		//HPが０になったら破壊する
		if ( _cardData._toughness == 0 ) { 
			Death( );	
		}
	}
	//------------------------------------------

	//回復--------------------------------------
	public void Recovery( int increasePoint ) {
		_cardData._toughness += increasePoint;

		if ( _cardData._toughness > _cardData._maxToughness ) {
			_cardData._toughness = _cardData._maxToughness;
		}
	}
	//------------------------------------------

	//破壊
	public void Death( ) {
		Destroy( this.gameObject );	
	}


	//プレイヤーによってカードの持っている向きを調整して返す処理------------------------------------
	public List< Field.DIRECTION > getDirections( string player, List< Field.DIRECTION > directions ) {
		if ( player == ConstantStorehouse.TAG_PLAYER1 ) {
			return directions;
		}

		List< Field.DIRECTION > player2Directions = new List< Field.DIRECTION >( directions.Count );
		for ( int i = 0; i < directions.Count; i++ ) { 
		
			switch( directions[ i ] ) { 
				case Field.DIRECTION.LEFT_FORWARD:
					player2Directions.Add( Field.DIRECTION.RIGHT_BACK );
					break;

				case Field.DIRECTION.FORWAED:
					player2Directions.Add( Field.DIRECTION.BACK );
					break;

				case Field.DIRECTION.RIGHT_FORWARD:
					player2Directions.Add( Field.DIRECTION.LEFT_BACK );
					break;

				case Field.DIRECTION.LEFT:
					player2Directions.Add( Field.DIRECTION.RIGHT );
					break;

				case Field.DIRECTION.RIGHT:
					player2Directions.Add( Field.DIRECTION.LEFT );
					break;

				case Field.DIRECTION.LEFT_BACK:
					player2Directions.Add( Field.DIRECTION.RIGHT_FORWARD );
					break;

				case Field.DIRECTION.BACK:
					player2Directions.Add( Field.DIRECTION.FORWAED );
					break;

				case Field.DIRECTION.RIGHT_BACK:
					player2Directions.Add( Field.DIRECTION.LEFT_FORWARD );
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
		attackPoint.text = _cardData._attack.ToString( );
		hitPoint.text = _cardData._toughness.ToString( );

		//位置をずらしている
		_details.transform.SetParent( _canvas.transform );
//		_details.transform.parent = _canvas.transform;//RectTransformを使っているGameObjectの子にする時はSetParentメゾット推奨だそうです（警告が出ます、UIスケーリング問題を防ぐためとか…）
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
